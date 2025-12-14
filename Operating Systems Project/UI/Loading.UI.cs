using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    /// <summary>
    /// Loading overlay UI: creates an overlay + centered card with a custom-drawn circular spinner.
    /// Use:
    ///     Loading.StartLoading(); // automatically finds Operating_Systems form
    ///     Loading.StartLoading(this, "Working...", "Please wait", translucency: 140);
    ///     Loading.StopLoading();
    /// </summary>
    internal partial class Loading
    {
        // shared UI fields (other partial file references the same names)
        private static Panel _overlay;
        private static Panel _card;
        private static Label _titleLabel;
        private static Label _messageLabel;
        private static Spinner _spinner;

        /// <summary>
        /// Start the loading overlay. If an overlay already exists, updates the message/title.
        /// </summary>
        /// <param name="parent">Parent form to attach to. If null, tries to find Operating_Systems or falls back to the first open form.</param>
        /// <param name="message">Message text shown under the title.</param>
        /// <param name="title">Title text shown above the message.</param>
        /// <param name="translucency">Alpha 0..255 to apply to Operating_Systems.Background for overlay dimming (default 160).</param>
        public static void StartLoading(Form parent = null, string message = "Loading...", string title = "Please wait", int translucency = 160)
        {
            // Resolve parent if not provided
            if (parent == null)
            {
                parent = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(f => f.GetType().Name == "Operating_Systems");
                if (parent == null && Application.OpenForms.Count > 0)
                    parent = Application.OpenForms[0];
            }

            if (parent == null) return;

            // If overlay already exists, just update text
            if (_overlay != null)
            {
                UpdateTitle(title);
                UpdateMessage(message);
                return;
            }

            void create()
            {
                // Build overlay color from Operating_Systems.Background with requested alpha
                Color baseBg = Operating_Systems.Background;
                int alpha = Math.Max(0, Math.Min(255, translucency));
                Color overlayColor = Color.FromArgb(alpha, baseBg.R, baseBg.G, baseBg.B);

                // Overlay panel covers the whole form
                _overlay = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = overlayColor,
                    Cursor = Cursors.WaitCursor,
                    Visible = true
                };

                // Prevent clicks from reaching underlying controls
                _overlay.Click += (s, e) => { /* swallow */ };

                // Card in center
                _card = new Panel
                {
                    Size = new Size(380, 110),
                    BackColor = overlayColor, //Operating_Systems.PanelColor,
                    BorderStyle = BorderStyle.None
                };

                // Center the card in the parent client area
                _card.Left = (parent.ClientSize.Width - _card.Width) / 2;
                _card.Top = (parent.ClientSize.Height - _card.Height) / 2;
                _card.Anchor = AnchorStyles.None;

                // Title label
                _titleLabel = new Label
                {
                    Text = title,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Operating_Systems.TextPrimary,
                    Dock = DockStyle.Top,
                    Height = 30
                };

                // Message label
                _messageLabel = new Label
                {
                    Text = message,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5f, FontStyle.Regular),
                    ForeColor = Operating_Systems.TextSecondary,
                    Dock = DockStyle.Top,
                    Height = 26
                };

                // Spinner (custom vector-drawn)
                _spinner = new Spinner
                {
                    Size = new Size(56, 56),
                    Anchor = AnchorStyles.None,
                    Segments = 12,
                    Thickness = 4,
                    Speed = 40,
                    SegmentColor = Operating_Systems.AccentBlue
                };

                // Left panel for spinner; right panel for labels
                var inner = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };
                var left = new Panel { Width = 80, Dock = DockStyle.Left, BackColor = Color.Transparent };
                var right = new Panel { Dock = DockStyle.Fill, BackColor = Color.Transparent };

                // Position spinner centrally inside left panel
                left.Controls.Add(_spinner);
                left.Resize += (s, e) =>
                {
                    _spinner.Left = Math.Max(8, (left.ClientSize.Width - _spinner.Width) / 2);
                    _spinner.Top = Math.Max(2, (left.ClientSize.Height - _spinner.Height) / 2);
                };
                // Trigger initial placement
                left.PerformLayout();

                // Add labels to right panel (order: title on top, message below)
                right.Controls.Add(_messageLabel);
                right.Controls.Add(_titleLabel);

                inner.Controls.Add(right);
                inner.Controls.Add(left);

                _card.Controls.Add(inner);

                // Add overlay and card to parent
                parent.Controls.Add(_overlay);
                parent.Controls.Add(_card);

                // Set stacking: overlay below card but both above other content
                _overlay.BringToFront();
                _card.BringToFront();

                // Keep card centered on parent resize
                parent.Resize += Parent_Resize;

                // Start spinner
                _spinner.Start();

                // Force immediate repaint
                _overlay.Refresh();
                _card.Refresh();
            }

            if (parent.InvokeRequired)
                parent.Invoke((Action)create);
            else
                create();
        }

        // Safely update title text
        private static void UpdateTitle(string title)
        {
            if (_titleLabel == null) return;
            var parent = _titleLabel.FindForm();
            if (parent != null && parent.InvokeRequired)
                parent.Invoke((Action)(() => _titleLabel.Text = title));
            else
                _titleLabel.Text = title;
        }

        // Safely update message text
        private static void UpdateMessage(string message)
        {
            if (_messageLabel == null) return;
            var parent = _messageLabel.FindForm();
            if (parent != null && parent.InvokeRequired)
                parent.Invoke((Action)(() => _messageLabel.Text = message));
            else
                _messageLabel.Text = message;
        }

        /// <summary>
        /// Lightweight custom spinner control — vector drawn, anti-aliased.
        /// No external assets required.
        /// </summary>
        private class Spinner : Control, IDisposable
        {
            private readonly Timer _timer;
            private float _angle;

            // customizable properties
            public int Segments { get; set; } = 12;
            public int Thickness { get; set; } = 4;
            /// <summary>Milliseconds between ticks (smaller = faster)</summary>
            public int Speed
            {
                get => _timer.Interval;
                set => _timer.Interval = Math.Max(8, value);
            }

            public Color SegmentColor { get; set; } = Operating_Systems.AccentBlue;

            public Spinner()
            {
                SetStyle(ControlStyles.AllPaintingInWmPaint |
                         ControlStyles.OptimizedDoubleBuffer |
                         ControlStyles.ResizeRedraw |
                         ControlStyles.UserPaint, true);

                Width = 56;
                Height = 56;

                _timer = new Timer { Interval = 40 };
                _timer.Tick += (s, e) =>
                {
                    _angle += 360f / Math.Max(1, Segments) * 0.8f;
                    if (_angle > 360f) _angle -= 360f;
                    Invalidate();
                };
            }

            public void Start() => _timer.Start();
            public void Stop() => _timer.Stop();

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);

                var g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;

                float cx = ClientSize.Width / 2f;
                float cy = ClientSize.Height / 2f;
                float radius = Math.Min(cx, cy) - Thickness - 1;

                // Draw segments with a fade tail effect
                for (int i = 0; i < Segments; i++)
                {
                    // compute segment position
                    float segmentAngle = _angle + i * (360f / Segments);
                    double rad = (segmentAngle - 90) * Math.PI / 180.0;

                    // compute intensity: head segments brighter, tail dimmer
                    int headIndex = 0;
                    int distanceFromHead = (Segments - i) % Segments;
                    float intensity = 1.0f - (distanceFromHead / (float)Segments);
                    intensity = 0.25f + intensity * 0.75f; // keep visible
                    int alpha = (int)(255 * intensity);
                    alpha = Math.Max(40, Math.Min(255, alpha));

                    using (var pen = new Pen(Color.FromArgb(alpha, SegmentColor), Thickness))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;

                        float sx = cx + (float)(radius * Math.Cos(rad));
                        float sy = cy + (float)(radius * Math.Sin(rad));
                        float ex = cx + (float)((radius - 6) * Math.Cos(rad));
                        float ey = cy + (float)((radius - 6) * Math.Sin(rad));

                        g.DrawLine(pen, ex, ey, sx, sy);
                    }
                }
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    try
                    {
                        _timer?.Stop();
                        _timer?.Dispose();
                    }
                    catch { }
                }
                base.Dispose(disposing);
            }
        }
    }
}
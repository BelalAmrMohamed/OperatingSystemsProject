using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace Operating_Systems_Project
{
    /// <summary>
    /// Lightweight, reusable loading overlay for the project.
    /// Usage:
    ///     Loading.StartLoading();           // auto-detects your main Operating_Systems form
    ///     Loading.StartLoading(this);       // explicitly pass a Form
    ///     Loading.StopLoading();
    ///
    /// The overlay fills the entire form (Dock = Fill) and uses the project's
    /// theme colors from Operating_Systems.*.
    /// </summary>
    public static class Loading
    {
        private static Panel _overlay;
        private static Panel _card;
        private static Label _titleLabel;
        private static Label _messageLabel;
        private static ProgressBar _marquee;

        /// <summary>
        /// Start the loading overlay. If parent is null the code will try to find
        /// the first open Operating_Systems form.
        /// </summary>
        public static void StartLoading(Form parent = null, string message = "Loading...")
        {
            //// Find parent if not provided
            if (parent == null)
            {
                parent = Application.OpenForms.Cast<Form>()
                    .FirstOrDefault(f => f.GetType().Name == "Operating_Systems");

                if (parent == null && Application.OpenForms.Count > 0)
                    parent = Application.OpenForms[0];
            }

            if (parent == null) return; // no window to attach to

            // If overlay already exists, just update the message
            if (_overlay != null)
            {
                UpdateMessage(message);
                return;
            }

            void create()
            {
                // Full-screen overlay
                _overlay = new Panel
                {
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(10, 10, 10), // subtle dark translucent feel
                    Cursor = Cursors.WaitCursor,
                    Visible = true
                };

                // Make overlay intercept all mouse events so underlying controls don't flicker
                _overlay.Click += (s, e) => { /* swallow clicks */ };

                // Center card
                _card = new Panel
                {
                    Size = new Size(420, 120),
                    BackColor = Operating_Systems.PanelColor,
                    Anchor = AnchorStyles.None
                };

                // Center the card manually (because Dock.Fill used on overlay)
                _card.Left = (parent.ClientSize.Width - _card.Width) / 2;
                _card.Top = (parent.ClientSize.Height - _card.Height) / 2;
                _card.Anchor = AnchorStyles.None;

                // Title
                _titleLabel = new Label
                {
                    Text = "Please wait",
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = Operating_Systems.MainFont,
                    ForeColor = Operating_Systems.TextPrimary,
                    Dock = DockStyle.Top,
                    Height = 36
                };

                // Message
                _messageLabel = new Label
                {
                    Text = message,
                    AutoSize = false,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = new Font(SystemFonts.DefaultFont.FontFamily, 9.5f, FontStyle.Regular),
                    ForeColor = Operating_Systems.TextSecondary,
                    Dock = DockStyle.Top,
                    Height = 28
                };

                // Marquee
                _marquee = new ProgressBar
                {
                    Style = ProgressBarStyle.Marquee,
                    MarqueeAnimationSpeed = 30,
                    Dock = DockStyle.Bottom,
                    Height = 18
                };

                // Add a small padding panel inside card to give spacing
                var inner = new Panel { Dock = DockStyle.Fill, Padding = new Padding(12) };
                inner.Controls.Add(_marquee);
                inner.Controls.Add(_messageLabel);
                inner.Controls.Add(_titleLabel);

                _card.Controls.Add(inner);

                // Add overlay and card to parent
                parent.Controls.Add(_overlay);
                parent.Controls.Add(_card);

                // Keep card above overlay
                _card.BringToFront();
                _overlay.BringToFront();
                _card.BringToFront();

                // Handle parent resize so the card stays centered
                parent.Resize += Parent_Resize;

                // Force redraw
                _overlay.Refresh();
                _card.Refresh();
            }

            // Ensure we run on UI thread
            if (parent.InvokeRequired)
                parent.Invoke((Action)create);
            else
                create();
        }

        private static void Parent_Resize(object sender, EventArgs e)
        {
            if (_card == null || !(sender is Form parent)) return;

            _card.Left = (parent.ClientSize.Width - _card.Width) / 2;
            _card.Top = (parent.ClientSize.Height - _card.Height) / 2;
        }

        /// <summary>
        /// Stop and remove the loading overlay if present.
        /// </summary>
        public static void StopLoading()
        {
            if (_overlay == null) return;

            // Find parent by walking up from card if possible
            Form parent = _card?.FindForm() ?? Application.OpenForms.Cast<Form>().FirstOrDefault(f => f.GetType().Name == "Operating_Systems");
            if (parent == null)
            {
                // try to clean up without parent
                _marquee?.Dispose();
                _messageLabel?.Dispose();
                _titleLabel?.Dispose();
                _card?.Dispose();
                _overlay?.Dispose();

                _marquee = null;
                _messageLabel = null;
                _titleLabel = null;
                _card = null;
                _overlay = null;
                return;
            }

            void destroy()
            {
                parent.Resize -= Parent_Resize;

                if (_card != null && parent.Controls.Contains(_card)) parent.Controls.Remove(_card);
                if (_overlay != null && parent.Controls.Contains(_overlay)) parent.Controls.Remove(_overlay);

                _marquee?.Dispose();
                _messageLabel?.Dispose();
                _titleLabel?.Dispose();
                _card?.Dispose();
                _overlay?.Dispose();

                _marquee = null;
                _messageLabel = null;
                _titleLabel = null;
                _card = null;
                _overlay = null;
            }

            if (parent.InvokeRequired)
                parent.Invoke((Action)destroy);
            else
                destroy();
        }

        /// <summary>
        /// Update the message text if overlay already exists.
        /// </summary>
        private static void UpdateMessage(string message)
        {
            if (_messageLabel == null) return;

            var parent = _messageLabel.FindForm();
            if (parent != null && parent.InvokeRequired)
            {
                parent.Invoke((Action)(() => _messageLabel.Text = message));
            }
            else
            {
                _messageLabel.Text = message;
            }
        }
    }
}
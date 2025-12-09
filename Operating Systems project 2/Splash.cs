using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    public static class Splash
    {
        private static Form _form;
        private static System.Windows.Forms.Timer _animTimer;
        private static int _spinnerAngle;
        private static int _progress; // 0 to 100
        private static string _statusText = "Initializing modules...";

        // Style Constants
        private static readonly Color ColorBg = Color.FromArgb(32, 32, 32);
        private static readonly Color ColorText = Color.FromArgb(240, 240, 240);
        private static readonly Color ColorAccent = Color.FromArgb(0, 122, 204); // VS Blue
        private static readonly Color ColorDim = Color.FromArgb(80, 80, 80);

        /// <summary>
        /// Show the splash screen on its own STA thread. Optionally provide an initTask that reports progress.
        /// The splash will attempt to match the main form's bounds (if available) and will activate the main form
        /// before closing to avoid the "minimizes after splash" issue.
        /// </summary>
        public static void ShowSplash(Func<IProgress<int>, Task> initTask = null, int minDisplayMs = 1500)
        {
            if (_form != null && !_form.IsDisposed) return;

            _spinnerAngle = 0;
            _progress = 0;

            // Capture the main form bounds on the caller thread (usually the main UI thread).
            // This is safer than trying to access Application.OpenForms from inside the new STA thread.
            Rectangle? mainBounds = null;
            try
            {
                var mainForm = Application.OpenForms.OfType<Operating_Systems>().FirstOrDefault();
                if (mainForm != null && mainForm.IsHandleCreated)
                {
                    // Capture location/size now
                    mainBounds = new Rectangle(mainForm.Location, mainForm.Size);
                }
            }
            catch
            {
                // If anything fails, we'll just center the splash (default behavior).
                mainBounds = null;
            }

            // Use a dedicated STA thread for the splash UI
            var thread = new Thread(() =>
            {
                _form = new InternalSplashForm();

                // If main bounds were captured, apply them so the splash overlays the main window.
                if (mainBounds.HasValue)
                {
                    _form.StartPosition = FormStartPosition.Manual;
                    _form.Size = mainBounds.Value.Size;
                    _form.Location = mainBounds.Value.Location;
                }

                // Animation timer runs on the splash's UI thread.
                _animTimer = new System.Windows.Forms.Timer { Interval = 15 };
                _animTimer.Tick += (s, e) =>
                {
                    _spinnerAngle = (_spinnerAngle + 4) % 360;
                    if (_form != null && !_form.IsDisposed)
                        _form.Invalidate();
                };
                _animTimer.Start();

                // Run message loop for the splash form only
                Application.Run(_form);

                // Clean up anim timer if thread exits for any reason
                try { _animTimer?.Stop(); }
                catch { }
            });

            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();

            // Wait until the splash handle is created to avoid races
            while (_form == null || !_form.IsHandleCreated)
            {
                Thread.Sleep(10);
            }

            // Perform initialization work on a ThreadPool thread; report progress to the splash.
            Task.Run(async () =>
            {
                var reporter = new Progress<int>(p => { _progress = p; });

                if (initTask != null)
                {
                    try
                    {
                        await initTask(reporter);
                    }
                    catch
                    {
                        // Swallow exceptions from initTask so splash can close gracefully.
                    }
                }
                else
                {
                    for (int i = 0; i <= 100; i++)
                    {
                        ((IProgress<int>)reporter).Report(i);
                        await Task.Delay(Math.Max(1, minDisplayMs / 100));
                    }
                }

                // Small buffer before fade
                await Task.Delay(200);

                if (_form != null && !_form.IsDisposed)
                {
                    try
                    {
                        // Ensure FadeOutAndClose runs on the splash UI thread
                        _form.Invoke(new Action(() => FadeOutAndClose()));
                    }
                    catch
                    {
                        // Form might already be closing; ignore.
                    }
                }
            });
        }

        public static void SetStatus(string text)
        {
            _statusText = text;
            // Force a repaint if the form is present
            try
            {
                if (_form != null && !_form.IsDisposed)
                {
                    if (_form.InvokeRequired) _form.BeginInvoke(new Action(() => _form.Invalidate()));
                    else _form.Invalidate();
                }
            }
            catch { }
        }

        private static void FadeOutAndClose()
        {
            var t = new System.Windows.Forms.Timer { Interval = 10 };
            t.Tick += (s, e) =>
            {
                if (_form == null || _form.IsDisposed) { t.Stop(); return; }

                // Reduce opacity smoothly
                _form.Opacity -= 0.05;
                if (_form.Opacity <= 0)
                {
                    t.Stop();
                    _animTimer?.Stop();

                    // FIX: Find and activate the main form before closing the splash form.
                    // We use Application.OpenForms to find the Operating_Systems main form and bring it to front.
                    try
                    {
                        foreach (Form f in Application.OpenForms)
                        {
                            if (f is Operating_Systems os)
                            {
                                // Ensure activation happens on the main form's owning thread
                                if (os.IsHandleCreated)
                                {
                                    if (os.InvokeRequired)
                                    {
                                        try
                                        {
                                            os.BeginInvoke(new Action(() =>
                                            {
                                                try { os.Show(); os.WindowState = FormWindowState.Normal; os.Activate(); }
                                                catch { }
                                            }));
                                        }
                                        catch { }
                                    }
                                    else
                                    {
                                        try { os.Show(); os.WindowState = FormWindowState.Normal; os.Activate(); }
                                        catch { }
                                    }
                                }
                                break;
                            }
                        }
                    }
                    catch
                    {
                        // If anything goes wrong, proceed to close splash anyway.
                    }

                    try
                    {
                        _form.Close();
                        _form.Dispose();
                    }
                    catch { }
                    finally
                    {
                        _form = null;
                    }
                }
            };
            t.Start();
        }

        // ---------------------------------------------------------
        // The Visual Form
        // ---------------------------------------------------------
        private class InternalSplashForm : Form
        {
            public InternalSplashForm()
            {
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.CenterScreen;
                Size = new Size(500, 300);
                BackColor = ColorBg;
                ShowInTaskbar = false;
                TopMost = true;
                DoubleBuffered = true; // Essential for smooth animation
                Opacity = 1.0;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                int cx = Width / 2;
                int cy = Height / 2;

                // 1. Draw Title
                using (var font = new Font("Segoe UI Light", 24))
                using (var brush = new SolidBrush(ColorText))
                {
                    var size = g.MeasureString("OPERATING SYSTEM", font);
                    g.DrawString("OPERATING SYSTEM", font, brush, cx - (size.Width / 2), 60);
                }

                // 2. Draw Status
                using (var font = new Font("Consolas", 9))
                using (var brush = new SolidBrush(Color.Gray))
                {
                    var text = $"{_statusText?.ToUpper() ?? string.Empty} [{_progress}%]";
                    var size = g.MeasureString(text, font);
                    g.DrawString(text, font, brush, cx - (size.Width / 2), 240);
                }

                // 3. Draw Modern Spinner (Center Ring)
                int r = 30; // radius
                Rectangle rect = new Rectangle(cx - r, cy - r, r * 2, r * 2);

                using (var penBg = new Pen(ColorDim, 3))
                using (var penActive = new Pen(ColorAccent, 3))
                {
                    // Track circle (background)
                    g.DrawEllipse(penBg, rect);

                    // Rotating Arc
                    penActive.StartCap = LineCap.Round;
                    penActive.EndCap = LineCap.Round;
                    g.DrawArc(penActive, rect, _spinnerAngle, 100);
                }

                // 4. Draw Slim Progress Line (Bottom)
                int barWidth = Math.Min(Width - 80, 300);
                int barHeight = 2;
                int barX = cx - (barWidth / 2);
                int barY = 220;

                // Empty bar
                using (var b = new SolidBrush(Color.FromArgb(50, 50, 50)))
                    g.FillRectangle(b, barX, barY, barWidth, barHeight);

                // Fill bar
                int fillWidth = (int)((_progress / 100.0f) * barWidth);
                using (var b = new SolidBrush(ColorAccent))
                    g.FillRectangle(b, barX, barY, fillWidth, barHeight);

                // 5. Draw Border (Subtle)
                using (var p = new Pen(Color.FromArgb(60, 60, 60), 1))
                    g.DrawRectangle(p, 0, 0, Width - 1, Height - 1);
            }
        }
    }
}

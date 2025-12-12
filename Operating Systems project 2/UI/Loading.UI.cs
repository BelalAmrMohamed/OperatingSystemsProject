using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    
    internal partial class Loading
    {
        /// <summary>
        /// Start the loading overlay. If parent is null the code will try to find
        /// the first open Operating_Systems form.
        /// </summary>

        private static Panel _overlay;
        private static Panel _card;
        private static Label _titleLabel;
        private static Label _messageLabel;
        private static ProgressBar _marquee;
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

    }
}

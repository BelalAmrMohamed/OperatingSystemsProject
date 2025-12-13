using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    public partial class Operating_Systems : Form
    {
        // ====== Dark Theme Colors ======
        public static Color Background = Color.FromArgb(32, 32, 32); // 13, 17, 23 // 32,32,32
        public static Color ButtonsPanelBackground = Color.FromArgb(25, 25, 25);
        public static Color PanelColor = Color.FromArgb(43, 43, 43);
        public static Color WMISmallPanelColor = Color.FromArgb(0, 30, 50);
        public static Color TextPrimary = Color.FromArgb(255, 255, 255);
        public static Color TextSecondary = Color.FromArgb(160, 160, 160);
        public static Color AccentGreen = Color.FromArgb(40, 167, 69);
        public static Color AccentBlue = Color.FromArgb(0, 122, 204);
        public static Color SuccessColor = Color.FromArgb(40, 200, 90);
        public static Color ErrorColor = Color.FromArgb(220, 53, 69);
        public static Color NeutralColor = Color.FromArgb(108, 117, 125);
        public static Color HighlightedButton = Color.FromArgb(230, 230, 130);
        public static Color HeaderColor = Color.FromArgb(230, 230, 130);
        public static Color DarkButton = Color.FromArgb(30, 32, 34);

        // Main Font
        public static readonly Font MainFont = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);

        #region DARK TITLE BAR

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Flags used to force a frame update
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_DRAWFRAME = SWP_FRAMECHANGED;

        /// <summary>
        /// Enable or disable the dark title bar for this form.
        /// Call from your theme switch: e.g. `DarkTitleBar(true)` or `DarkTitleBar(false)`.
        /// Safe to call at any time: if the window handle isn't created yet the call will be scheduled
        /// to run after handle creation.
        /// </summary>
        public void DarkTitleBar(bool enable)
        {
            // If the handle isn't created yet, delay applying until it's ready.
            if (!this.IsHandleCreated)
            {
                // Use a one-time handler so we don't attach multiple times
                void Handler(object s, EventArgs e)
                {
                    this.HandleCreated -= Handler;
                    ApplyDarkTitleBar(enable);
                }
                this.HandleCreated += Handler;
                return;
            }

            ApplyDarkTitleBar(enable);
        }

        private void ApplyDarkTitleBar(bool enable)
        {
            try
            {
                int useDark = enable ? 1 : 0;

                // Try the newer attribute first; if it fails, try the older attribute.
                int hr = DwmSetWindowAttribute(this.Handle,
                    DWMWA_USE_IMMERSIVE_DARK_MODE,
                    ref useDark,
                    sizeof(int));

                if (hr != 0)
                {
                    // fallback for older builds
                    DwmSetWindowAttribute(this.Handle,
                        DWMWA_USE_IMMERSIVE_DARK_MODE_OLD,
                        ref useDark,
                        sizeof(int));
                }

                // Force Windows to redraw the non-client area (title bar)
                // Use SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED
                uint flags = SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED;
                SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, flags);
            }
            catch
            {
                // Ignore: running on OS that doesn't support these attributes is fine.
            }
        }

        #endregion

        public static void RefreshBackgroundColor(Operating_Systems OS)
        {
            OS.BackColor = Background;
            OS.ButtonsPanel.BackColor = ButtonsPanelBackground;

            RefreshButton(OS.General_IO_Button);
            RefreshButton(OS.FileWriter_Button);
            RefreshButton(OS.FileReader_Button);
            RefreshButton(OS.FolderWatcher_Button);
            RefreshButton(OS.StopwatchTimer_Button);
            RefreshButton(OS.WMI_Button);
            RefreshButton(OS.PowerOptions_Button);
            RefreshButton(OS.Settings_Button);
        }

        private static void RefreshButton(Button b)
        {
            b.ForeColor = TextPrimary;
            b.FlatAppearance.MouseOverBackColor = Experimental.IsDarkMode() ? Color.FromArgb(15,15,15) : Color.FromArgb(240,240,240); // Hover
            b.FlatAppearance.MouseDownBackColor = Experimental.IsDarkMode() ? Color.Black : Color.White; // When clicked
        }

        public void ToggleBackgroundImage()
        {
            if (this.BackgroundImage == null) // If there's no image show image, else disable image
                this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.red_background2;
            else this.BackgroundImage = null;
        }
        public bool IsBackgroundImageEnabled()
        {
            if (this.BackgroundImage == null)
                return false;
            else return true;
        }

        //private async void Operating_Systems_Shown(object sender, EventArgs e)
        //{
        //    Loading.StartLoading(this);
        //    await Task.Delay(1000); // tiny yield so the overlay paints (can be 1-100ms)

        //    // Run UI initialization (these must run on UI thread if they create controls)
        //    About.ShowAbout(this);
        //    ToggleBackgroundImage();

        //    Loading.StopLoading();
        //}
    }
}

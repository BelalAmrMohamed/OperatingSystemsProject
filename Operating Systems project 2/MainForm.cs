using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Operating_Systems_Project
{
    public partial class Operating_Systems : Form
    {
        // ===== Version (loaded from App.config at runtime) =====
        // Note: cannot be const because it's read at runtime.
        public static readonly string APP_VERSION;

        // ====== Dark Theme Colors ======
        // Change the IsDarkMode() Method at the bottom of the file if you'll change this value
        public static Color Background = Color.FromArgb(32, 32, 32); // 32, 32, 32  // 13, 17, 23 
        public static Color ButtonsPanelBackground = Color.FromArgb(25, 25, 25);
        public static Color PanelColor = Color.FromArgb(43, 43, 43); // 43, 43, 43     // 21, 27, 35
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

        // Dimensions
        public static readonly Size ContentSize = new Size(1152, 592);
        public static readonly int ContentWidth = 1110;
        public static readonly int ContentHeight = 533;

        public static readonly Size formSize = new Size(1152, 592);
        public static readonly int formWidth = 1152;
        public static readonly int formHeight = 592;

        // Static constructor: runs once per AppDomain before any instance is created
        static Operating_Systems()
        {
            const string defaultVersion = "1.0.0"; // If you see this in RunTime it means there's an error
            string configPath = Path.Combine(Application.StartupPath, "App.config");

            try
            {
                if (File.Exists(configPath))
                {
                    var doc = XDocument.Load(configPath);

                    // 1) Look for a <Version> element anywhere: <About><Version>1.0.0/Version></About>
                    var versionElem = doc.Descendants("Version").FirstOrDefault();
                    if (versionElem != null && !string.IsNullOrWhiteSpace(versionElem.Value))
                    {
                        APP_VERSION = versionElem.Value.Trim();
                        return;
                    }

                    // 2) Look for appSettings style: <add key="Version" value="4.2.3" />
                    var addElem = doc.Descendants("add")
                                     .FirstOrDefault(x =>
                                         string.Equals((string)x.Attribute("key"), "Version", StringComparison.OrdinalIgnoreCase) ||
                                         string.Equals((string)x.Attribute("key"), "AppVersion", StringComparison.OrdinalIgnoreCase));
                    if (addElem != null)
                    {
                        var val = (string)addElem.Attribute("value");
                        if (!string.IsNullOrWhiteSpace(val))
                        {
                            APP_VERSION = val.Trim();
                            return;
                        }
                    }

                    // 3) No version found — fall through to default
                }
            }
            catch
            {
                // ignore parsing errors, fall back to default
            }

            APP_VERSION = defaultVersion;
        }

        // DWM / Title bar interop constants & imports
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
                uint flags = SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED;
                SetWindowPos(this.Handle, IntPtr.Zero, 0, 0, 0, 0, flags);
            }
            catch
            {
                // Ignore: running on OS that doesn't support these attributes is fine.
            }
        }

        #endregion

        public Operating_Systems()
        {
            InitializeComponent();

            // Enable double buffering for the main container to reduce flicker
            try
            {
                typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    .SetValue(MainContainer, true);
            }
            catch
            {
                // ignore if reflection fails
            }
        }

        private void Operating_Systems_Load(object sender, EventArgs e)
        {
            // Enable dark title bar and show a small loading overlay while opening the default page
            DarkTitleBar(true);

            this.BeginInvoke(new Action(async () =>
            {
                Loading.StartLoading(this);
                await Task.Delay(300); // allow overlay to render briefly
                General_IO.ShowGeneral_IO(this);
                Loading.StopLoading();
            }));
        }

        // ===== Button handlers =====
        private void Menu_Button_Click(object sender, EventArgs e)
        {
            ButtonsPanel.Visible = true;
        }

        private void Menu_Button2_Click(object sender, EventArgs e)
        {
            ButtonsPanel.Visible = false;
        }

        private void General_IO_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => General_IO.ShowGeneral_IO(os));
        }

        private void FileWriter_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => FileWriter.ShowFileWriter(os));
        }

        private void FileReader_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => FileReader.ShowFileReader(os));
        }

        private void FolderWatcher_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => FolderMonitor.ShowFolderMonitor(os));
        }

        private void StopwatchTimer_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => TimersAccuracy.ShowTimer(os));
        }

        private void WMI_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => WMI.ShowWMI(os));
        }

        private void PowerOptions_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => PowerOptions.ShowPowerOption(os));
        }

        private void Settings_Button_Click(object sender, EventArgs e)
        {
            ShowView(os => Settings.ShowSettings(os));
        }

        private async void Hacker_Box_Click(object sender, EventArgs e)
        {
            Loading.StartLoading();

            await Task.Delay(1000);

            ClearContent();
            ButtonsPanel.Visible = false;
            Experimental.ShowExperimental(this);

            Loading.StopLoading();
        }


        // ========== Helper methods ==========

        private void ShowView(Action<Operating_Systems> action)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            action?.Invoke(this);
        }

        public void AddToMainContainer(Control c)
        {
            MainContainer.Controls.Add(c);
        }

        public void ClearContent()
        {
            MainContainer.Controls.Clear();
        }

        // ===== Theme helpers =====

        public static void RefreshBackgroundColor(Operating_Systems OS)
        {
            if (OS == null) return;

            OS.BackColor = Background;
            if (OS.ButtonsPanel != null) OS.ButtonsPanel.BackColor = ButtonsPanelBackground;

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

            b.FlatAppearance.BorderSize = 0;
            b.FlatAppearance.MouseOverBackColor = IsDarkMode() ? Color.FromArgb(15, 15, 15) : Color.FromArgb(240, 240, 240);
            b.FlatAppearance.MouseDownBackColor = IsDarkMode() ? Color.Black : Color.White;
        }

        private int count = 0;
        public void ToggleBackgroundImage()
        {
            if (this.BackgroundImage == null) // If there's no image show image, else disable image
            {
                Experimental.ApplyDarkMode(this);
                switch (count)
                {
                    case 0:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.IslamicWallpaper_2;

                        break;

                    case 1:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.AlAqsa;
                        break;

                    case 2:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.LightThemeBackground;
                        break;

                    case 3:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.red_background;
                        break;

                    case 4:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.CoffeAndCodeBackground;
                        break;

                    case 5:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.IslamicCats;
                        break;

                    case 6:
                        this.BackgroundImage = global::Operating_Systems_Project.Properties.Resources.IslamicWallpaper;
                        break;
                    default:
                        break;
                }
                count = (count + 1) % 7;
            }
            else
            {
                this.BackgroundImage = null;
            }
        }

        public bool IsBackgroundImageEnabled()
        {
            return this.BackgroundImage != null;
        }

        public static bool IsDarkMode()
        {
            return Background.R == 32;
        }
    }
}
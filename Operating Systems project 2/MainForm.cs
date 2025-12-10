//using System;
//using System.Drawing;
//using System.Runtime.CompilerServices;
//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Windows.Forms;

//namespace Operating_Systems_Project
//{
//    public partial class Operating_Systems : Form
//    {
//        /// <summary>
//        /// مشروع انظمة التشغيل. الملفات المختلفة
//        /// "FileWriter.cs", "FileReader.cs", "FolderMonitor.cs", "Timer.cs", "WMI.cs", "PowerOptions.cs", "About.cs"
//        /// ======================================================================================================================================
//        /// </summary>



//        // ====== Dark Theme Colors ======
//        public static Color Background = Color.FromArgb(32, 32, 32);
//        public static Color ButtonsPanelBackground = Color.FromArgb(25,25,25);
//        public static Color PanelColor = Color.FromArgb(43, 43, 43);
//        public static Color WMISmallPanelColor = Color.FromArgb(0, 30, 50);
//        public static Color TextPrimary = Color.FromArgb(255, 255, 255);
//        public static Color TextSecondary = Color.FromArgb(160, 160, 160);
//        public static Color AccentGreen = Color.FromArgb(40, 167, 69);
//        public static Color AccentBlue = Color.FromArgb(0, 122, 204);
//        public static Color SuccessColor = Color.FromArgb(40, 200, 90);
//        public static Color ErrorColor = Color.FromArgb(220, 53, 69);
//        public static Color NeutralColor = Color.FromArgb(108, 117, 125);
//        public static Color HighlightedButton = Color.FromArgb(230, 230, 130);
//        public static Color HeaderColor = Color.FromArgb(230, 230, 130);
//        public static Color DarkButton = Color.FromArgb(30, 32, 34);

//        public static void RefreshBackgroundColor(Operating_Systems os)
//        { 
//            os.BackColor = Background;
//            os.ButtonsPanel.BackColor = ButtonsPanelBackground;

//            os.FileWriter_Button.ForeColor = TextPrimary;
//            os.FileReader_Button.ForeColor = TextPrimary;
//            os.FolderWatcher_Button.ForeColor = TextPrimary;
//            os.StopwatchTimer_Button.ForeColor = TextPrimary;
//            os.WMI_Button.ForeColor = TextPrimary;
//            os.PowerOptions_Button.ForeColor = TextPrimary;
//            os.About_Button.ForeColor = TextPrimary;
//        }

//        public void ToggleBackgroundImage(bool EnableImage = false)
//        {
//            if (!EnableImage) this.BackgroundImage = null;
//            else this.BackgroundImage = global::Operating_Systems_project_2.Properties.Resources.red_background2;
//        }

//        public bool IsBackgroundImageEnabled()
//        {
//            if (this.BackgroundImage == null)
//                return false;
//            else return true;
//        }

//        // Main Font
//        public static readonly Font MainFont = new Font("Segoe UI Semibold", 13F, FontStyle.Bold);

//        // Dimensions
//        public static readonly Size ContentSize = new Size(1152, 592);
//        public static readonly int ContentWidth = 1110;
//        public static readonly int ContentHeight = 533;

//        public static readonly Size formSize = new Size(1152, 592);
//        public static readonly int formWidth = 1152;
//        public static readonly int formHeight = 592;

//        public Operating_Systems()
//        {
//            InitializeComponent();
//        }

//        #region DARK TITLE BAR
//        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
//        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;

//        [DllImport("dwmapi.dll")]
//        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

//        protected override void OnHandleCreated(EventArgs e)
//        {
//            base.OnHandleCreated(e);
//            EnableDarkTitleBar();
//        }

//        private void EnableDarkTitleBar()
//        {
//            try
//            {
//                int useDark = 1;

//                // Try the newer attribute first
//                int result = DwmSetWindowAttribute(this.Handle,
//                    DWMWA_USE_IMMERSIVE_DARK_MODE,
//                    ref useDark,
//                    sizeof(int));

//                // Try fallback if first fails
//                if (result != 0)
//                {
//                    DwmSetWindowAttribute(this.Handle,
//                        DWMWA_USE_IMMERSIVE_DARK_MODE_OLD,
//                        ref useDark,
//                        sizeof(int));
//                }
//            }
//            catch
//            {
//                // Ignored: OS might not support dark mode
//            }
//        }
//        #endregion

//        private void Operating_Systems_Load(object sender, EventArgs e)
//        {
//            Splash.ShowSplash();
//            //WMI.ShowWMI(this);
//            About.ShowAbout(this);
//            ToggleBackgroundImage();
//        }

//        private void Menu_Button_Click(object sender, EventArgs e)
//        {
//            ButtonsPanel.Visible = true;
//        }

//        private void Menu_Button2_Click(object sender, EventArgs e)
//        {
//            ButtonsPanel.Visible = false;
//        }

//        private void FileWriter_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            FileWriter.ShowFileWriter(this);
//        }

//        private void FileReader_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            FileReader.ShowFileReader(this);
//        }

//        private void FolderWatcher_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            FolderMonitor.ShowFolderMonitor(this);
//        }

//        private void StopwatchTimer_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            Timer.ShowTimer(this);
//        }

//        private void WMI_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            WMI.ShowWMI(this);
//        }

//        private void PowerOptions_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            PowerOptions.ShowPowerOption(this);
//        }
//        private void Hacker_Box_Click(object sender, EventArgs e)
//        {
//            Splash.ShowSplash();

//            ClearContent();
//            ButtonsPanel.Visible = false;
//            Settings.ShowSettings(this);
//        }

//        private void About_Button_Click(object sender, EventArgs e)
//        {
//            ClearContent();
//            ButtonsPanel.Visible = false;
//            About.ShowAbout(this);
//        }

//        // Helper methods
//        public void AddToMainContainer(Control c)
//        {
//            MainContainer.Controls.Add(c);
//        }

//        public void ClearContent()
//        { MainContainer.Controls.Clear(); }
//    }
//}

using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
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
        public static Color Background = Color.FromArgb(32, 32, 32);
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

        public Operating_Systems()
        {
            InitializeComponent();
        }

        #region DARK TITLE BAR
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            EnableDarkTitleBar();
        }

        private void EnableDarkTitleBar()
        {
            try
            {
                int useDark = 1;

                // Try the newer attribute first
                int result = DwmSetWindowAttribute(this.Handle,
                    DWMWA_USE_IMMERSIVE_DARK_MODE,
                    ref useDark,
                    sizeof(int));

                // Try fallback if first fails
                if (result != 0)
                {
                    DwmSetWindowAttribute(this.Handle,
                        DWMWA_USE_IMMERSIVE_DARK_MODE_OLD,
                        ref useDark,
                        sizeof(int));
                }
            }
            catch
            {
                // Ignored: OS might not support dark mode
            }
        }
        #endregion

        // For 'Settings'
        public static void RefreshBackgroundColor(Operating_Systems os)
        {
            os.BackColor = Background;
            os.ButtonsPanel.BackColor = ButtonsPanelBackground;

            os.FileWriter_Button.ForeColor = TextPrimary;
            os.FileReader_Button.ForeColor = TextPrimary;
            os.FolderWatcher_Button.ForeColor = TextPrimary;
            os.StopwatchTimer_Button.ForeColor = TextPrimary;
            os.WMI_Button.ForeColor = TextPrimary;
            os.PowerOptions_Button.ForeColor = TextPrimary;
            os.About_Button.ForeColor = TextPrimary;
        }
        public void ToggleBackgroundImage(bool EnableImage = false)
        {
            if (!EnableImage) this.BackgroundImage = null;
            else this.BackgroundImage = global::Operating_Systems_project_2.Properties.Resources.red_background2;
        }
        public bool IsBackgroundImageEnabled()
        {
            if (this.BackgroundImage == null)
                return false;
            else return true;
        }


        private void Operating_Systems_Load(object sender, EventArgs e)
        {
            Splash.ShowSplash();
            //WMI.ShowWMI(this);
            About.ShowAbout(this);
            ToggleBackgroundImage();
        }

        private void Menu_Button_Click(object sender, EventArgs e)
        {
            ButtonsPanel.Visible = true;
        }

        private void Menu_Button2_Click(object sender, EventArgs e)
        {
            ButtonsPanel.Visible = false;
        }

        private void FileWriter_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            FileWriter.ShowFileWriter(this);
        }

        private void FileReader_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            FileReader.ShowFileReader(this);
        }

        private void FolderWatcher_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            FolderMonitor.ShowFolderMonitor(this);
        }

        private void StopwatchTimer_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            Timer.ShowTimer(this);
        }

        private void WMI_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            WMI.ShowWMI(this);
        }

        private void PowerOptions_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            PowerOptions.ShowPowerOption(this);
        }
        private void Hacker_Box_Click(object sender, EventArgs e)
        {
            Splash.ShowSplash();

            ClearContent();
            ButtonsPanel.Visible = false;
            Settings.ShowSettings(this);
        }

        private void About_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;
            About.ShowAbout(this);
        }

        // Helper methods
        public void AddToMainContainer(Control c)
        {
            MainContainer.Controls.Add(c);
        }

        public void ClearContent()
        { MainContainer.Controls.Clear(); }
    }
}

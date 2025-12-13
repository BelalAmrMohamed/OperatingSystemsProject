using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    public partial class Operating_Systems : Form
    {
        // ===== Version (loaded from App.config at runtime) =====
        // Note: cannot be const because it's read at runtime.
        public static readonly string APP_VERSION;

        // Dimensions
        public static readonly Size ContentSize = new Size(1152, 592);
        public static readonly int ContentWidth = 1110;
        public static readonly int ContentHeight = 533;

        public static readonly Size formSize = new Size(1152, 592);
        public static readonly int formWidth = 1152;
        public static readonly int formHeight = 592;

        public Operating_Systems()
        {
            InitializeComponent();
            typeof(Panel).GetProperty("DoubleBuffered", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            .SetValue(MainContainer, true);
        }

        private void Operating_Systems_Load(object sender, EventArgs e)
        {
            DarkTitleBar(true);
            this.BeginInvoke(new Action(async () =>
            {
                Loading.StartLoading(this);
                await Task.Delay(1000);
                General_IO.ShowGeneral_IO(this);
                Loading.StopLoading();
            }));
        }

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

        private void Hacker_Box_Click(object sender, EventArgs e)
        {
            this.BeginInvoke(new Action(async () =>
            {
                Loading.StartLoading();
                await Task.Delay(1000);

                ClearContent();
                ButtonsPanel.Visible = false;
                Experimental.ShowSettings(this);

                Loading.StopLoading();
            }));
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
        { MainContainer.Controls.Clear(); }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    public partial class Operating_Systems : Form
    {
        /// <summary>
        /// مشروع انظمة التشغيل. الملفات المختلفة
        /// "FileWriter.cs", "FileReader.cs", "FolderMonitor.cs", "Timer.cs", "WMI.cs", "PowerOptions.cs", "About.cs"
        /// ======================================================================================================================================
        /// </summary>

        // جميع الألوان المستخدمة في المشروع ، بما في ذلك الألوان المستخدمة في الملفات الأخرى
        public static readonly Color Background = Color.FromArgb(31, 31, 31);
        public static readonly Color PanelColor = Color.FromArgb(43, 43, 43);
        public static readonly Color TextPrimary = Color.FromArgb(230, 230, 230);
        public static readonly Color TextSecondary = Color.FromArgb(160, 160, 160);
        public static readonly Color AccentGreen = Color.FromArgb(40, 167, 69);
        public static readonly Color AccentBlue = Color.FromArgb(0, 122, 204);
        public static readonly Color SuccessColor = Color.FromArgb(40, 200, 90);
        public static readonly Color ErrorColor = Color.FromArgb(220, 53, 69);
        public static readonly Color NeutralColor = Color.FromArgb(108, 117, 125);
        public static readonly Color HighlightedButton = Color.FromArgb(61, 61, 61);

        public static readonly Size ContentSize = new Size(1152, 592);
        public static readonly int ContentWidth = 1110;
        public static readonly int ContentHeight = 533;

        // أبعاد المشروع ، هيتم استخدامها لو "تغيير الحجم" اتفعل ؛ لأن حاليا الحجم متثبت        
        public static readonly Size formSize = new Size(1152, 592);
        public static readonly int formWidth = 1152;
        public static readonly int formHeight = 592;

        public Operating_Systems()
        {
            InitializeComponent();
            FileWriter.ShowFileWriter(this);
        }

        private void Menu_Opened_Click(object sender, EventArgs e)
        {
            ButtonsPanel.Visible = false;
        }

        private void Menu_Closed_Click(object sender, EventArgs e)
        {
            ButtonsPanel.Visible = true;
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
            //"⏱️ Stopwatch Timer";
            //"Precision stopwatch. Know the difference between different timers.";
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

        private void About_Button_Click(object sender, EventArgs e)
        {
            ClearContent();
            ButtonsPanel.Visible = false;            
            About.ShowAbout(this);
        }

        public void AddToMainContainer(Control c)
        {
            MainContainer.Controls.Add(c);
        }

        public void ClearContent()
        {
            if (MainContainer == null) return;

            for (int i = MainContainer.Controls.Count - 1; i >= 0; i--)
            {
                Control ctrl = MainContainer.Controls[i];
                if (ctrl != Menu_Closed) MainContainer.Controls.RemoveAt(i);  // ✅ keep.
            }
        }
    }
}
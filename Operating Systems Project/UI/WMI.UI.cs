using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        private static Panel resultsPanel;
        private static Label copyMessageLabel;

        private static readonly string DefaultCopyMessage = "Double click any box to copy its information";
        private static readonly string SuccessfulCopyingMessage = "✓ Content copied to your clipboard";
        public static void ShowWMI(Operating_Systems OS)
        {
            // Layout constants
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0;

            Label HeaderLabel = new Label
            {
                Text = "💻 WMI",
                Font = Operating_Systems.MainFont, // new Font("Segoe UI Semibold", 13F, FontStyle.Bold),

                MinimumSize = new Size(200, 30),
                Location = new Point(457, 8),
                Size = new Size(200, 30),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.HeaderColor,
            };
            Label SubHeaderLabel = new Label
            {
                Text = "Windows Management Instrumentation. many queries.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                MinimumSize = new Size(300, 20),
                Location = new Point(414, 38),
                Size = new Size(300, 20),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.TextPrimary,
            };
            OS.AddToMainContainer(HeaderLabel);
            OS.AddToMainContainer(SubHeaderLabel);
            currentY += 32;

            // --- 4. Query Label ---
            Label queryLabel = new Label
            {
                Text = "Choose a query",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            OS.AddToMainContainer(queryLabel);
            currentY += queryLabel.Height + 6;

            // Query Selector
            ComboBox querySelector = new ComboBox
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 36),
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Operating_Systems.TextPrimary,
                FlatStyle = FlatStyle.Popup,
                DropDownStyle = ComboBoxStyle.DropDownList,
            };
            OS.AddToMainContainer(querySelector);
            currentY += querySelector.Height + VerticalSpacing;

            querySelector.Items.Add("Win32_ComputerSystem (Rename Computer)");
            querySelector.Items.Add("Win32_PerfFormattedData_PerfOS_Memory");
            querySelector.Items.Add("Win32_LogicalDisk (Logical Disk)");
            querySelector.Items.Add("Win32_Desktop (Specific info)");
            querySelector.Items.Add("Win32_ComputerSystem (Type)");
            querySelector.Items.Add("Win32_ComputerSystemProduct");
            querySelector.Items.Add("Win32_Desktop (All info)");
            querySelector.Items.Add("Win32_BootConfiguration");
            querySelector.Items.Add("Win32_CodecFile (Video)");
            querySelector.Items.Add("Win32_CodecFile (Audio)");
            querySelector.Items.Add("Win32_CodecFile (All)");
            querySelector.Items.Add("Win32_Service (running)");
            querySelector.Items.Add("Win32_Service (stopped)");
            querySelector.Items.Add("Win32_OperatingSystem");
            querySelector.Items.Add("Win32_ComputerSystem");
            querySelector.Items.Add("Win32_Service (all)");
            querySelector.Items.Add("Win32_Group (Local)");
            querySelector.Items.Add("Win32_Group (all)");
            querySelector.Items.Add("Win32_LogicalDisk");
            querySelector.Items.Add("Win32_UserAccount");
            querySelector.Items.Add("Win32_CDROMDrive");
            querySelector.Items.Add("Win32_Processor");
            querySelector.Items.Add("Win32_Battery");
            querySelector.Items.Add("Win32_Share");

            if (querySelector.Items.Count > 0) querySelector.SelectedIndex = 0;

            Button runQueryButton = new Button
            {
                Text = "Run query",
                Size = new Size(150, 42),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0),
                Location = new Point(0, 100),
            };
            runQueryButton.FlatAppearance.BorderSize = 0;
            OS.AddToMainContainer(runQueryButton);

            copyMessageLabel = new Label
            {
                Text = DefaultCopyMessage,
                AutoSize = true,
                ForeColor = Operating_Systems.TextSecondary,
                Font = new Font("Segoe UI Semibold", 10F, FontStyle.Italic),
                Location = new Point(runQueryButton.Width + 10, 112),
                Visible = false,
            };
            OS.AddToMainContainer(copyMessageLabel);

            currentY += runQueryButton.Height + 25;

            resultsPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 360),
                BackColor = Operating_Systems.PanelColor,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 360),
                BorderStyle = BorderStyle.FixedSingle,
            };
            OS.AddToMainContainer(resultsPanel);

            runQueryButton.Click += (s, e) =>
            {
                resultsPanel.Controls.Clear();
                resultsPanel.Height = 360;
                if (copyMessageLabel.Visible != true) copyMessageLabel.Visible = true;
                ShowQuery(querySelector.SelectedItem.ToString()); // like: Win32_CodecFile (All)
            };
        }
    }
}
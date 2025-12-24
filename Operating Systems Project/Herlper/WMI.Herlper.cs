using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        public static Color SmallPanelColor = Color.FromArgb(0, 30, 50);

        private static void ShowQuery_MultiTextBoxes(string[] info)
        {
            const int containerWidth = 1103;
            const int verticalSpacing = 3;
            int currentY = verticalSpacing;

            foreach (string text in info)
            {
                var resultsBox = new Label
                {
                    Font = new Font("Segoe UI Semibold", 11F),
                    ForeColor = Operating_Systems.TextPrimary,
                    BackColor = SmallPanelColor, // 1- 59, 60, 109 // 2- AccentBlue // 3- White and black // 4- 108, 250, 125 // 5- 0, 30, 50
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(30, 30, 30, 30),
                    Width = containerWidth - 25,
                    Location = new Point(3, currentY),
                    Text = text
                };

                resultsBox.Height = GetBoxHeight(text);

                resultsBox.DoubleClick += (s, e) =>
                {
                    Clipboard.SetText(resultsBox.Text);
                    _ = Display_CopiedSuccessfully(); // The '_ = ' is a fix to the restricted 'await' problem
                };

                resultsPanel.Controls.Add(resultsBox);
                currentY += resultsBox.Height + verticalSpacing;
            }
            Panel ExtraSpaceAtTheBottom = new Panel
            {
                Size = new Size(resultsPanel.Width - 100, verticalSpacing),
                Location = new Point(0, currentY - verticalSpacing),
            };
            resultsPanel.Controls.Add(ExtraSpaceAtTheBottom);
        }

        /// ======================
        /// Helper Methods
        /// ======================

        private static int GetBoxHeight(string text)
        {
            // The height is: number of lines * 21.
            // 21 is one line's height.
            // 425 is the maximum height.
            return Math.Min(text.Split('\n').Length * 21, 425);
        }

        private static async Task Display_CopiedSuccessfully()
        {
            copyMessageLabel.Text = SuccessfulCopyingMessage;
            copyMessageLabel.ForeColor = Color.Green;

            await Task.Delay(2000); // Non-blocking wait for 3 seconds

            copyMessageLabel.Text = DefaultCopyMessage;
            copyMessageLabel.ForeColor = Operating_Systems.TextPrimary;
        }

        /// =============================
        /// WMI.UI.cs helpers
        /// =============================
        private static string[] GetMatchingQuery(string query, bool allInfo)
        {
            switch (query)
            {
                case "Win32_PerfFormattedData_PerfOS_Memory":
                    return new[] { GetMemoryInformation() };

                case "Win32_LogicalDisk":
                    return GetLogicalDiskInfo();

                case "Win32_Desktop WHERE Name = '.Default'":
                    return new[] { GetDesktopInfo() };

                case "Win32_ComputerSystem":
                    return new[] { GetComputerSystemInfo() };

                case "Win32_ComputerSystemProduct":
                    return new[] { GetProductInfo() };

                case "Win32_BootConfiguration":
                    return new[] { GetBootConfiguration() };

                case "Win32_CodecFile":
                    return Codec();

                case "Win32_CodecFile WHERE Group='Video'":
                    return CodecVideo();

                case "Win32_CodecFile WHERE Group='Audio'":
                    return CodecAudio();

                case "Win32_Service":
                    return GetAllServices();

                case "Win32_Service WHERE State='running'":
                    return GetRunningServices();

                case "Win32_Service WHERE State='stopped'":
                    return GetStoppedServices();

                case "Win32_OperatingSystem":
                    return new[] { Get_OS_Info() };

                case "Win32_Group":
                    return ListOfAllUserGroups();

                case "Win32_Group WHERE LocalAccount = 'true'":
                    return ListOfUserGroups();

                case "Win32_UserAccount":
                    return GetUserAccounts();

                case "Win32_CDROMDrive":
                    return GET_CD_RomInfo();

                case "Win32_Processor":
                    return new[] { GetNumberOfCores() };

                case "Win32_Battery":
                    return GetBatteryInfo();

                case "Win32_Share":
                    return GetListOfFileShares();

                default:
                    return allInfo ? GetAllQueryInfo(query) : GetQueryInfo(query);                    
            }
        }
    }
}
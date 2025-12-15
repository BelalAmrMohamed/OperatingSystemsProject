using System;
using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        public static Color SmallPanelColor = Color.FromArgb(0, 30, 50);
        private static void ShowQuery(string query)
        {
            switch (query)
            {
                case "Win32_ComputerSystem (Rename Computer)": // All of these methods return an array
                    ShowQuery_MultiTextBoxes(RenameComputer());
                    break;

                case "Win32_PerfFormattedData_PerfOS_Memory":
                    ShowQuery_MultiTextBoxes(GetMemoryInformation());
                    break;

                case "Win32_LogicalDisk (Logical Disk)":
                    ShowQuery_MultiTextBoxes(GetLogicalDiskInfo());
                    break;

                case "Win32_Desktop (Specific info)":
                    ShowQuery_MultiTextBoxes(GetDesktopInfo());
                    break;

                case "Win32_ComputerSystem (Type)":
                    ShowQuery_MultiTextBoxes(GetComputerType());
                    break;

                case "Win32_ComputerSystemProduct":
                    ShowQuery_MultiTextBoxes(GetProductInfo());
                    break;

                case "Win32_Desktop (All info)":
                    ShowQuery_MultiTextBoxes(GetAllDesktopInfo());
                    break;

                case "Win32_BootConfiguration":
                    ShowQuery_MultiTextBoxes(GetBootConfiguration());
                    break;
                case "Win32_CodecFile (Video)":
                    ShowQuery_MultiTextBoxes(CodecVideo());
                    break;

                case "Win32_CodecFile (Audio)":
                    ShowQuery_MultiTextBoxes(CodecAudio());
                    break;

                case "Win32_CodecFile (All)":
                    ShowQuery_MultiTextBoxes(Codec());
                    break;

                case "Win32_Service (running)":
                    ShowQuery_MultiTextBoxes(GetRunningServices());
                    break;

                case "Win32_Service (stopped)":
                    ShowQuery_MultiTextBoxes(GetStoppedServices());
                    break;

                case "Win32_OperatingSystem":
                    ShowQuery_MultiTextBoxes(Get_OS_Info());
                    break;

                case "Win32_ComputerSystem":
                    ShowQuery_MultiTextBoxes(GetComputerSystemInfo());
                    break;

                case "Win32_Service (all)":
                    ShowQuery_MultiTextBoxes(GetAllServices());
                    break;

                case "Win32_Group (Local)":
                    ShowQuery_MultiTextBoxes(ListOfUserGroups());
                    break;
                case "Win32_Group (all)":
                    ShowQuery_MultiTextBoxes(ListOfAllUserGroups());
                    break;

                case "Win32_LogicalDisk":
                    ShowQuery_MultiTextBoxes(GetPartitionsInfo());
                    break;

                case "Win32_UserAccount":
                    ShowQuery_MultiTextBoxes(GetUserAccounts());
                    break;

                case "Win32_CDROMDrive":
                    ShowQuery_MultiTextBoxes(GET_CD_RomInfo());
                    break;

                case "Win32_Processor":
                    ShowQuery_MultiTextBoxes(GetProcessorInfo());
                    break;

                case "Win32_Battery":
                    ShowQuery_MultiTextBoxes(GetBatteryInfo());
                    break;

                case "Win32_Share":
                    ShowQuery_MultiTextBoxes(GetListOfFileShares());
                    break;
            }
        }

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

                        MessageBox.Show(
                            "Text copied to your clipboard", 
                            "Operating Systems app", 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);                    
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
        
        public static int GetBoxHeight(string text)
        {
            // The height is `number of lines * 21`. 21 is the line height.
            // 425 is the maximum height.
            return Math.Min(text.Split('\n').Length * 21, 425);            
        }
    }
}
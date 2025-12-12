using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        private static void ShowQuery_MultiTextBoxes(Operating_Systems OS, Func<object> action)
        {
            object result = action();

            // Normalize acceptable return types to an enumerable of strings
            IEnumerable<string> infoEnumerable;
            if (result is string single) infoEnumerable = new[] { single };
            else if (result is string[] arr) infoEnumerable = arr;
            else if (result is IEnumerable<string> ie) infoEnumerable = ie;
            else
                throw new InvalidOperationException("The provided method must return string or string[] or IEnumerable<string>");

            const int containerWidth = 1103;
            const int verticalSpacing = 3;
            const int lineHeight = 21;
            const int minLines = 1;
            const int maxHeight = 400;
            int currentY = 3;

            foreach (var raw in infoEnumerable)
            {
                string text = (raw ?? string.Empty).Replace("\n", Environment.NewLine);

                var resultsTextBox = new TextBox
                {
                    Font = new Font("Segoe UI Semibold", 11F),
                    ForeColor = Operating_Systems.TextPrimary,
                    BackColor = Operating_Systems.WMISmallPanelColor, // 1- 59, 60, 109 // 2- AccentBlue // 3- White and black // 4- 108, 250, 125 // 5- 0, 30, 50
                    Multiline = true,
                    BorderStyle = BorderStyle.FixedSingle,
                    ReadOnly = true,
                    WordWrap = true,
                    ScrollBars = ScrollBars.None,
                    Margin = new Padding(30, 30, 30, 30),
                    Width = containerWidth - 25,
                    Location = new Point(3, currentY),
                    Text = text
                };

                int availableWidth = Math.Max(1, resultsTextBox.Width - 8);

                int totalLines = 0;
                string[] paragraphs = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

                foreach (var para in paragraphs)
                {
                    if (string.IsNullOrEmpty(para))
                    {
                        totalLines += 1;
                        continue;
                    }

                    Size measured = TextRenderer.MeasureText(
                        para,
                        resultsTextBox.Font,
                        new Size(int.MaxValue, int.MaxValue),
                        TextFormatFlags.SingleLine | TextFormatFlags.NoPadding);

                    int paragraphLines = (int)Math.Ceiling((double)measured.Width / availableWidth);
                    if (paragraphLines < 1) paragraphLines = 1;
                    totalLines += paragraphLines;
                }

                totalLines = Math.Max(minLines, totalLines);

                int desiredHeight = totalLines * lineHeight;

                if (desiredHeight > maxHeight)
                {
                    resultsTextBox.Height = maxHeight;
                    resultsTextBox.ScrollBars = ScrollBars.Vertical;
                }
                else
                {
                    resultsTextBox.Height = desiredHeight;
                }

                resultsPanel.Controls.Add(resultsTextBox);
                currentY += resultsTextBox.Height + verticalSpacing;
            }
            Panel ExtraSpaceAtTheBottom = new Panel
            {
                Size = new Size(resultsPanel.Width - 100, 3),
                Location = new Point(0, currentY - verticalSpacing),
            };
            resultsPanel.Controls.Add(ExtraSpaceAtTheBottom);
        }

        private static void ShowQuery(Operating_Systems OS, string query)
        {
            switch (query)
            {
                case "Win32_ComputerSystem (Rename Computer)":
                    ShowQuery_MultiTextBoxes(OS, RenameComputer);
                    break;

                case "Win32_PerfFormattedData_PerfOS_Memory":
                    ShowQuery_MultiTextBoxes(OS, GetMemoryInformation);
                    break;

                case "Win32_LogicalDisk (Logical Disk)":
                    ShowQuery_MultiTextBoxes(OS, GetLogicalDiskInfo);
                    break;

                case "Win32_Desktop (Specific info)":
                    ShowQuery_MultiTextBoxes(OS, GetDesktopInfo);
                    break;

                case "Win32_ComputerSystem (Type)":
                    ShowQuery_MultiTextBoxes(OS, GetComputerType);
                    break;

                case "Win32_ComputerSystemProduct":
                    ShowQuery_MultiTextBoxes(OS, GetProductInfo);
                    break;

                case "Win32_Desktop (All info)":
                    ShowQuery_MultiTextBoxes(OS, GetAllDesktopInfo);
                    break;

                case "Win32_BootConfiguration":
                    ShowQuery_MultiTextBoxes(OS, GetBootConfiguration);
                    break;
                case "Win32_CodecFile (Video)":
                    ShowQuery_MultiTextBoxes(OS, CodecVideo);
                    break;

                case "Win32_CodecFile (Audio)":
                    ShowQuery_MultiTextBoxes(OS, CodecAudio);
                    break;

                case "Win32_CodecFile (All)":
                    ShowQuery_MultiTextBoxes(OS, Codec);
                    break;

                case "Win32_Service (running)":
                    ShowQuery_MultiTextBoxes(OS, GetRunningServices);
                    break;

                case "Win32_Service (stopped)":
                    ShowQuery_MultiTextBoxes(OS, GetStoppedServices);
                    break;

                case "Win32_OperatingSystem":
                    ShowQuery_MultiTextBoxes(OS, Get_OS_Info);
                    break;

                case "Win32_ComputerSystem":
                    ShowQuery_MultiTextBoxes(OS, GetComputerSystemInfo);
                    break;

                case "Win32_Service (all)":
                    ShowQuery_MultiTextBoxes(OS, GetAllServices);
                    break;

                case "Win32_Group (Local)":
                    ShowQuery_MultiTextBoxes(OS, ListOfUserGroups);
                    break;
                case "Win32_Group (all)":
                    ShowQuery_MultiTextBoxes(OS, ListOfAllUserGroups);
                    break;

                case "Win32_LogicalDisk":
                    ShowQuery_MultiTextBoxes(OS, GetPartitionsInfo);
                    break;

                case "Win32_UserAccount":
                    ShowQuery_MultiTextBoxes(OS, GetUserAccounts);
                    break;

                case "Win32_CDROMDrive":
                    ShowQuery_MultiTextBoxes(OS, GET_CD_RomInfo);
                    break;

                case "Win32_Processor":
                    ShowQuery_MultiTextBoxes(OS, GetProcessorInfo);
                    break;

                case "Win32_Battery":
                    ShowQuery_MultiTextBoxes(OS, GetBatteryInfo);
                    break;

                case "Win32_Share":
                    ShowQuery_MultiTextBoxes(OS, GetListOfFileShares);
                    break;
            }
        }
    }
}

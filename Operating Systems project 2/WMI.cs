using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class WMI
    {
        #region WMI Query Methods

        /// <summary>
        /// ======= Methods that need to be fixed =======
        /// Win32_Battery (one object doesn't work, the commented one)
        /// Win32_Service (doesn't work when state is set to "running" or "stopped")
        /// Win32_CDROMDrive (doesn't show anything)
        /// </summary>
        /// <returns></returns>

        private static string[] GetPartitionsInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Drive Name:",-15} {obj["Name"]}\n";
                info[index] += $"{"ID:",-15} {obj["DeviceID"]}\n";
                info[index] += $"{"File system:",-15} {obj["FileSystem"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }

        private static string[] GetLogicalDiskInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection collection = searcher.Get();

            const double BytesInGB = 1024.0 * 1024.0 * 1024.0;
            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                // If Size or FreeSpace could be null, Convert.ToDouble may throw — kept same behavior as original code.
                double freeSpaceGB = obj["FreeSpace"] != null ? Convert.ToDouble(obj["FreeSpace"]) / BytesInGB : 0.0;
                double diskSizeGB = obj["Size"] != null ? Convert.ToDouble(obj["Size"]) / BytesInGB : 0.0;

                info[index] = string.Empty;

                info[index] += $"{"Name:",-15} {obj["DeviceID"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}\n";
                info[index] += $"{"Free space:",-15} {Math.Round(freeSpaceGB, 2)} GB\n";
                info[index] += $"{"Disk size:",-15} {Math.Round(diskSizeGB, 2)} GB"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }


        private static string[] GetListOfFileShares()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Share");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Name:",-15} {obj["Name"]}\n";
                info[index] += $"{"Path:",-15} {obj["Path"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}";

                index++;
            }
            return info;
        }


        private static string[] GetAllServices()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Service Name:",-15} {obj["DisplayName"]}\n";
                info[index] += $"{"Start Mode:",-15} {obj["StartMode"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }

        private static string[] GetRunningServices()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service WHERE State='running'");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Service Name:",-15} {obj["DisplayName"]}\n";
                info[index] += $"{"Start Mode:",-15} {obj["StartMode"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }

        private static string[] GetStoppedServices()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Service WHERE State='stopped'");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Service Name:",-15} {obj["DisplayName"]}\n";
                info[index] += $"{"Start Mode:",-15} {obj["StartMode"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }

        private static string[] GetUserAccounts()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"User name:",-15} {obj["Name"]}\n";
                info[index] += $"{"Domain:",-15} {obj["Domain"]}\n";
                info[index] += $"{"Status:",-15} {obj["Status"]}\n";
                info[index] += $"{"Disabled:",-15} {obj["Disabled"]}\n";
                info[index] += $"{"Local account:",-15} {obj["LocalAccount"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }


        private static string GetComputerSystemInfo()
        {
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            string info = string.Empty;

            foreach (ManagementObject obj in Searcher.Get())
            {
                info += $"{"Computer Name:",-22} {obj["Name"]}\n";
                info += $"{"Domain:",-22} {obj["Domain"]}\n";
                info += $"{"Model:",-22} {obj["Model"]}\n";
                info += $"{"Manufacturer:",-22} {obj["Manufacturer"]}\n";
                info += $"{"Total Physical Memory:",-22} {Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024.0 * 1024.0 * 1024.0), 2)} GB\n";
                info += $"{"System Type:",-22} {obj["SystemType"]}\n";
                info += $"{"Workgroup/Domain Join:",-22} {obj["Workgroup"]}\n";
                info += $"{"Type:",-22} {GetComputerType()}";
            }
            return info;
        }

        private static string GetComputerType()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            string type = "Type: ";
            foreach (ManagementObject obj in searcher.Get())
            {
                switch (Convert.ToInt32(obj["DomainRole"]))
                {
                    case 0:
                        type += "Standalone Workstation";
                        break;
                    case 1:
                        type += "Member Workstation"; // Represents a member of a domain
                        break;
                    case 2:
                        type += "Primary Domain Controller";
                        break;
                    case 3:
                        type += "Backup Domain Controller";
                        break;
                    case 4:
                        type += "Standalone Server"; // Standalone server that's not part of a domain
                        break;
                    case 5:
                        type += "Member Server"; // Server that is a member of a domain
                        break;
                    default:
                        type += "Role Undefined (Code: " + Convert.ToInt32(obj["DomainRole"]) + ")";
                        break;
                }
            }
            return type;
        }

        private static string GetProductInfo()
        {
            ManagementObjectSearcher os = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            string info = string.Empty;

            foreach (ManagementObject obj in os.Get())
            {
                info += $"{"Manufacturer:",-20} {obj["Vendor"]}\n";
                info += $"{"UUID:",-20} {obj["UUID"]}\n";
                info += $"{"Name:",-20} {obj["Name"]}\n";
                info += $"{"Identifying Number:",-20} {obj["IdentifyingNumber"]}";
            }
            return info.ToString();
        }

        private static string GetProcessorInfo()
        {
            ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            string info = string.Empty;

            foreach (ManagementObject obj in cpuSearcher.Get())
            {
                info += $"Number of Cores: {obj["NumberOfCores"]}";
            }
            return info;
        }

        private static string Get_OS_Info()
        {
            ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            string info = string.Empty;

            foreach (ManagementObject obj in osSearcher.Get())
            {
                info += $"{"Name:",-20}{obj["Caption"]}\n";
                info += $"{"Version:",-20}{obj["Version"]}\n";
                info += $"{"Manufacturer:",-20}{obj["Manufacturer"]}\n";
                info += $"{"Windows Directory:",-20}{obj["WindowsDirectory"]}";
            }
            return info.ToString();
        }

        private static string GetDesktopInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Desktop WHERE Name = '.Default'");
            string info = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info += $"{"Desktop Name:",-20} {obj["Name"]}\n";
                info += $"{"Icon Title Size:",-20} {obj["IconTitleSize"]}\n";
                info += $"{"Wallpaper Stretched:",-20} {obj["WallpaperStretched"]}\n";
                info += $"{"Is there a screen saver:",-20} {obj["ScreenSaverActive"]}";

                try
                {
                    if (obj["ScreenSaverActive"].ToString() != "False")
                    {
                        info += $"\n{"Screen Saver time out:",-20} {obj["ScreenSaverTimeout"]}";
                    }
                }
                catch (Exception ex) { }
            }
            return info.ToString();
        }

        private static string GetAllDesktopInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Desktop WHERE Name = '.Default'");
            string info = string.Empty;
            foreach (ManagementObject obj in searcher.Get())
            {
                foreach (PropertyData prop in obj.Properties)
                {
                    info += $"{prop.Name} : {prop.Value}\n";
                }
            }
            return info;
        }

        private static string GetMemoryInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Memory");
            string info = string.Empty;
            foreach (ManagementObject obj in searcher.Get())
            {
                info += $"{"Available MBs:",-20} {obj["AvailableMbytes"]}\n";
                info += $"{"Cache Bytes:",-20} {obj["CacheBytes"]}\n";
                info += $"{"Committed Bytes:",-20} {obj["CommittedBytes"]}\n";
                info += $"{"Commit Limit:",-20} {obj["CommitLimit"]}";
            }
            return info.ToString();
        }

        private static string[] GET_CD_RomInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_CDROMDrive");
            ManagementObjectCollection collection = searcher.Get();
            if (collection.Count == 0) return new[] { "No CD/DVD/Virtual drives detected on this system." };
            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Description:",-15} {obj["Description"]}\n";
                info[index] += $"{"Drive:",-15} {obj["Drive"]}\n";
                info[index] += $"{"Media Type:",-15} {obj["MediaType"]}\n";
                info[index] += $"{"Size:",-15} {obj["Size"]}\n";
                info[index] += $"{"Transfer Rate:",-15} {obj["TransferRate"]}";

                index++;
            }
            return info;
        }

        private static string GetBootConfiguration()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BootConfiguration");
            ManagementObjectCollection collection = searcher.Get();

            string info = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info += $"{"BootDirectory:",-20} {obj["BootDirectory"]}\n";
                info += $"{"Description:",-20} {obj["Description"]}\n";
                info += $"{"Scratch Directory:",-20} {obj["ScratchDirectory"]}\n";
                info += $"{"Temp Directory:",-20} {obj["TempDirectory"]}";
            }
            return info;
        }

        private static string[] GetBatteryInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
            ManagementObjectCollection collection = searcher.Get();
            if (collection.Count == 0) return new[] { "No Win32_Battery instances found (likely a desktop PC)." };

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Device ID:",-25} {obj["DeviceID"]}\n";
                info[index] += $"{"Design Capacity:",-25} {obj["DesignCapacity"]} mWh\n";
                info[index] += $"{"Full Charge Capacity:",-25} {obj["FullChargeCapacity"]} mWh\n";
                info[index] += $"{"Estimated Run Time:",-25} {obj["EstimatedRunTime"]} minutes\n";
                //info[index] += $"{"Remaining Capacity:",-25} {obj["RemainingCapacity"]} mWh\n";
                info[index] += $"{"Battery Status Code:",-25} {obj["BatteryStatus"]}";

                index++;
            }
            return info;
        }

        private static string RenameComputer()
        {
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

            //object[] newName = { name };
            //foreach (ManagementObject obj in searcher.Get())
            //{
            //    obj.InvokeMethod("Rename", newName);
            //}
            return "Win32_ComputerSystem: Renaming is currently disabled";
        }

        /// <summary>
        /// New. The 10th section
        /// </summary>
        private static string[] ListOfUserGroups()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Group WHERE LocalAccount = 'true'");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Group name:",-15} {obj["Name"]}\n";
                info[index] += $"{"Domain:",-15} {obj["Domain"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}\n";
                info[index] += $"{"SID:",-15} {obj["SID"]}\n";
                info[index] += $"{"SID Type:",-15} {obj["SIDType"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }
        private static string[] ListOfAllUserGroups()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Group");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Group name:",-15} {obj["Name"]}\n";
                info[index] += $"{"Domain:",-15} {obj["Domain"]}\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}\n";
                info[index] += $"{"SID:",-15} {obj["SID"]}\n";
                info[index] += $"{"SID Type:",-15} {obj["SIDType"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }

        private static string[] Codec()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_CodecFile"); //'Video' 'Audio'
            ManagementObjectCollection collection = searcher.Get();
            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Codec Name:",-15} {obj["Caption"]}\n";
                info[index] += $"{"Codec Category:",-15} {obj["Group"]}\n";
                info[index] += $"{"File Path:",-15} {obj["Path"]}\n";
                info[index] += $"{"Size (bytes):",-15} {obj["FileSize"]}\n";
                info[index] += $"{"Compressed:",-15} {obj["Compressed"]}\n";
                info[index] += $"{"Encrypted:",-15} {obj["Encrypted"]}\n";
                info[index] += $"{"Readable:",-15} {obj["Readable"]}\n";
                info[index] += $"{"Writeable:",-15} {obj["Writeable"]}";

                index++;
            }
            return info;
        }

        private static string[] CodecVideo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_CodecFile WHERE Group='Video'");
            ManagementObjectCollection collection = searcher.Get();
            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Codec Name:",-15} {obj["Caption"]}\n";
                info[index] += $"{"Codec Category:",-15} {obj["Group"]}\n";
                info[index] += $"{"File Path:",-15} {obj["Path"]}\n";
                info[index] += $"{"Size (bytes):",-15} {obj["FileSize"]}\n";
                info[index] += $"{"Compressed:",-15} {obj["Compressed"]}\n";
                info[index] += $"{"Encrypted:",-15} {obj["Encrypted"]}\n";
                info[index] += $"{"Readable:",-15} {obj["Readable"]}\n";
                info[index] += $"{"Writeable:",-15} {obj["Writeable"]}";

                index++;
            }
            return info;
        }
        private static string[] CodecAudio()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_CodecFile WHERE Group='Audio'");
            ManagementObjectCollection collection = searcher.Get();
            string[] info = new string[collection.Count];
            int index = 0;
            foreach (ManagementObject obj in searcher.Get())
            {
                info[index] = string.Empty;

                info[index] += $"{"Codec Name:",-15} {obj["Caption"]}\n";
                info[index] += $"{"Codec Category:",-15} {obj["Group"]}\n";
                info[index] += $"{"File Path:",-15} {obj["Path"]}\n";
                info[index] += $"{"Size (bytes):",-15} {obj["FileSize"]}\n";
                info[index] += $"{"Compressed:",-15} {obj["Compressed"]}\n";
                info[index] += $"{"Encrypted:",-15} {obj["Encrypted"]}\n";
                info[index] += $"{"Readable:",-15} {obj["Readable"]}\n";
                info[index] += $"{"Writeable:",-15} {obj["Writeable"]}";

                index++;
            }
            return info;
        }

        #endregion


        public static Panel resultsPanel;
        public static void ShowWMI(Operating_Systems OperatingSystems)
        {
            // Layout constants
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0;

            Label HeaderLabel = new Label
            {
                Text = "💻 WMI",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                MinimumSize = new Size(200, 30),
                Location = new Point(457, 8),
                Size = new Size(200, 30),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.YellowHeader,
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
            OperatingSystems.AddToMainContainer(HeaderLabel);
            OperatingSystems.AddToMainContainer(SubHeaderLabel);
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
            OperatingSystems.AddToMainContainer(queryLabel);
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
            OperatingSystems.AddToMainContainer(querySelector);
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
            currentY += runQueryButton.Height + 25;
            OperatingSystems.AddToMainContainer(runQueryButton);

            resultsPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 360),
                BackColor = Operating_Systems.PanelColor,
                AutoScroll = true,
                AutoScrollMinSize = new Size(0, 360),
                BorderStyle = BorderStyle.FixedSingle,
            };
            OperatingSystems.AddToMainContainer(resultsPanel);

            runQueryButton.Click += (s, e) =>
            {
                resultsPanel.Controls.Clear();
                resultsPanel.Height = 360;
                ShowQuery(OperatingSystems, querySelector.SelectedItem.ToString());
            };
        }

        #region UI Helper Methods
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
                    ForeColor = Color.White, // Color.FromArgb(0, 0, 0), 
                    BackColor = Color.FromArgb(0, 30, 50), // 1- 59, 60, 109 // 2- AccentBlue // 3- White and black // 4- 108, 250, 125 // 5- 0, 30, 50
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
        #endregion
    }
}
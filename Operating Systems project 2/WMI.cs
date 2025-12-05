using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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
        /// Win32_Service (doesn't work when state is set to anything)
        /// Win32_CDROMDrive (doesn't show anything)
        /// </summary>
        /// <returns></returns>

        private static string[] GetPartitionsInfo() // This is merged with 'GetLogicalDiskInfo()'
        {
            // 'collection' is for getting the number that we will set the StringBuilder with.
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection collection = searcher.Get();

            StringBuilder[] info = new StringBuilder[collection.Count];
            int index = 0;
            const double BytesInGB = 1024.0 * 1024.0 * 1024.0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = new StringBuilder();

                info[index].AppendLine($"{"Drive:",-15} {obj["Name"]}");
                info[index].AppendLine($"{"ID:",-15} {obj["DeviceID"]}"); // This is from
                info[index].AppendLine($"{"File system:",-15} {obj["FileSystem"]}");
                info[index].AppendLine($"{"Description:",-15} {obj["Description"]}");

                double size = Convert.ToDouble(obj["Size"]) / BytesInGB;
                double free = Convert.ToDouble(obj["FreeSpace"]) / BytesInGB;

                info[index].AppendLine($"{"Size:",-15} {Math.Round(size, 2)} GB");
                info[index].AppendLine($"{"Free space:",-15} {Math.Round(free, 2)} GB");

                index++;
            }
            return info.Select(sb => sb.ToString()).ToArray();
        }
        private static string GetLogicalDiskInfo()  // Useless, since GetPartitionInfo has the same info and more
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            StringBuilder info = new StringBuilder();
            const double BytesInGB = 1024.0 * 1024.0 * 1024.0;

            foreach (ManagementObject obj in searcher.Get())
            {
                double freeSpaceGB = Convert.ToDouble(obj["FreeSpace"]) / BytesInGB;
                double diskSizeGB = Convert.ToDouble(obj["Size"]) / BytesInGB;

                info.AppendLine($"{"Name:",-15} {obj["DeviceID"]}");
                info.AppendLine($"{"Description:",-15} {obj["Description"]}");

                info.AppendLine($"{"Free space:",-15} {Math.Round(freeSpaceGB, 2)} GB");
                info.AppendLine($"{"Disk size:",-15} {Math.Round(diskSizeGB, 2)} GB");
            }
            return info.ToString();
        }
        private static string GetPartitionsInfoSimple() // Simple version, this is merged with 'GetLogicalDiskInfo()'
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");

            string info = string.Empty;
            const double BytesInGB = 1024.0 * 1024.0 * 1024.0;

            foreach (ManagementObject obj in searcher.Get())
            {
                // Calculate Size and Free space in GB
                double size = Convert.ToDouble(obj["Size"]) / BytesInGB;
                double free = Convert.ToDouble(obj["FreeSpace"]) / BytesInGB;

                info += $"{"Drive:",-15} {obj["Name"]}\n";
                info += $"{"ID:",-15} {obj["DeviceID"]}\n";
                info += $"{"File system:",-15} {obj["FileSystem"]}\n";
                info += $"{"Description:",-15} {obj["Description"]}\n";
                info += $"{"Size:",-15} {Math.Round(size, 2)} GB\n";
                info += $"{"Free space:",-15} {Math.Round(free, 2)} GB\n";
            }
            return info;
        }

        private static string[] GetListOfFileShares()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Share");
            ManagementObjectCollection collection = searcher.Get();

            StringBuilder[] info = new StringBuilder[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[index] = new StringBuilder();

                info[index].AppendLine($"{"Name:",-15} {obj["Name"]}");
                info[index].AppendLine($"{"Path:",-15} {obj["Path"]}");
                info[index].AppendLine($"{"Description:",-15} {obj["Description"]}");

                index++;
            }
            return info.Select(sb => sb.ToString()).ToArray();
        }

        private static string[] GetServices(string state = "")
        {
            string whereClause;

            if (!string.IsNullOrWhiteSpace(state))
                whereClause = $" WHERE State='{state}'";

            else whereClause = string.Empty;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Service{state}");
            ManagementObjectCollection collection = searcher.Get();

            StringBuilder[] info = new StringBuilder[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[index] = new StringBuilder();

                info[index].AppendLine($"{"Service Name:",-15} {obj["DisplayName"]}");
                info[index].AppendLine($"{"Start Mode:",-15} {obj["StartMode"]}");
                info[index].AppendLine($"{"Description:",-15} {obj["Description"]}");

                index++;
            }
            return info.Select(sb => sb.ToString()).ToArray();
        }

        private static string[] GetUserAccounts()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_UserAccount");
            ManagementObjectCollection collection = searcher.Get();

            StringBuilder[] info = new StringBuilder[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = new StringBuilder();

                info[index].AppendLine($"{"User name:",-15} {obj["Name"]}");
                info[index].AppendLine($"{"Domain:",-15} {obj["Domain"]}");
                info[index].AppendLine($"{"Status:",-15} {obj["Status"]}");
                info[index].AppendLine($"{"Disabled:",-15} {obj["Disabled"]}");
                info[index].AppendLine($"{"Local account:",-15} {obj["LocalAccount"]}");

                index++;
            }
            return info.Select(sb => sb.ToString()).ToArray();
        }

        private static string GetComputerSystemInfo()
        {
            ManagementObjectSearcher Searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in Searcher.Get())
            {
                info.AppendLine($"{"Computer Name:",-22} {obj["Name"]}");
                info.AppendLine($"{"Domain:",-22} {obj["Domain"]}");
                info.AppendLine($"{"Model:",-22} {obj["Model"]}");
                info.AppendLine($"{"Manufacturer:",-22} {obj["Manufacturer"]}");
                info.AppendLine($"{"Total Physical Memory:",-22} {Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024.0 * 1024.0 * 1024.0), 2)} GB");
                info.AppendLine($"{"System Type:",-22} {obj["SystemType"]}");
                info.AppendLine($"{"Workgroup/Domain Join:",-22} {obj["Workgroup"]}");
                info.AppendLine($"{"Type:",-22} {GetComputerType()}");
            }
            return info.ToString();
        }

        private static string GetComputerType()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            StringBuilder type = new StringBuilder(30);
            type.Append("Type: ");
            foreach (ManagementObject obj in searcher.Get())
            {
                switch (Convert.ToInt32(obj["DomainRole"]))
                {
                    case 0:
                        type.Append("Standalone Workstation");
                        break;
                    case 1:
                        type.Append("Member Workstation"); // Represents a member of a domain
                        break;
                    case 2:
                        type.Append("Primary Domain Controller");
                        break;
                    case 3:
                        type.Append("Backup Domain Controller");
                        break;
                    case 4:
                        type.Append("Standalone Server"); // Standalone server that's not part of a domain
                        break;
                    case 5:
                        type.Append("Member Server"); // Server that is a member of a domain
                        break;
                    default:
                        type.Append("Role Undefined (Code: " + Convert.ToInt32(obj["DomainRole"]) + ")");
                        break;
                }
            }
            return type.ToString();
        }

        private static string GetProductInfo()
        {
            ManagementObjectSearcher os = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in os.Get())
            {
                info.AppendLine($"{"Manufacturer:",-20} {obj["Vendor"]}\n");
                info.AppendLine($"{"UUID:",-20} {obj["UUID"]}\n");
                info.AppendLine($"{"Name:",-20} {obj["Name"]}\n");
                info.AppendLine($"{"Identifying Number:",-20} {obj["IdentifyingNumber"]}\n");
            }
            return info.ToString();
        }

        private static string GetProcessorInfo()
        {
            ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in cpuSearcher.Get())
            {
                info.AppendLine($"Number of Cores: {obj["NumberOfCores"]}");
            }
            return info.ToString();
        }

        private static string Get_OS_Info()
        {
            ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in osSearcher.Get())
            {
                info.AppendLine($"{"Name:",-20}{obj["Caption"]}");
                info.AppendLine($"{"Version:",-20}{obj["Version"]}");
                info.AppendLine($"{"Manufacturer:",-20}{obj["Manufacturer"]}");
                info.AppendLine($"{"Windows Directory:",-20}{obj["WindowsDirectory"]}");
            }
            return info.ToString();
        }

        private static string GetDesktopInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Desktop WHERE Name = '.Default'");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in searcher.Get())
            {
                info.AppendLine($"{"Desktop Name:",-20} {obj["Name"]}");
                info.AppendLine($"{"Icon Title Size:",-20} {obj["IconTitleSize"]}");
                info.AppendLine($"{"Wallpaper Stretched:",-20} {obj["WallpaperStretched"]}");
                info.AppendLine($"{"Is there a screen saver:",-20} {obj["ScreenSaverActive"]}");

                try
                {
                    if (obj["ScreenSaverActive"].ToString() != "False")
                    {
                        info.AppendLine($"{"Screen Saver time out:",-20} {obj["ScreenSaverTimeout"]}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return info.ToString();
        }

        private static string GetAllDesktopInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Desktop WHERE Name = '.Default'");
            StringBuilder allInfo = new StringBuilder();
            foreach (ManagementObject obj in searcher.Get())
            {
                foreach (PropertyData prop in obj.Properties)
                {
                    allInfo.AppendLine($"{prop.Name} : {prop.Value}");
                }
            }
            return allInfo.ToString();
        }

        private static string GetMemoryInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Memory");
            StringBuilder info = new StringBuilder();
            foreach (ManagementObject obj in searcher.Get())
            {
                info.AppendLine($"{"Available MBs:",-20} {obj["AvailableMbytes"]}");
                info.AppendLine($"{"Cache Bytes:",-20} {obj["CacheBytes"]}");
                info.AppendLine($"{"Committed Bytes:",-20} {obj["CommittedBytes"]}");
                info.AppendLine($"{"Commit Limit:",-20} {obj["CommitLimit"]}");
            }
            return info.ToString();
        }

        private static string GET_CD_RomInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_CDROMDrive");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in searcher.Get())
            {
                info.AppendLine($"{"Description:",-15} {obj["Description"]}");
                info.AppendLine($"{"Drive:",-15} {obj["Drive"]}");
                info.AppendLine($"{"Media Type:",-15} {obj["MediaType"]}");
                info.AppendLine($"{"Size:",-15} {obj["Size"]}");
                info.AppendLine($"{"Transfer Rate:",-15} {obj["TransferRate"]}");
            }
            return info.ToString();
        }

        private static string GetBootConfiguration()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BootConfiguration");
            StringBuilder info = new StringBuilder();

            foreach (ManagementObject obj in searcher.Get())
            {
                info.AppendLine($"{"BootDirectory:",-20} {obj["BootDirectory"]}");
                info.AppendLine($"{"Description:",-20} {obj["Description"]}");
                info.AppendLine($"{"Scratch Directory:",-20} {obj["ScratchDirectory"]}");
                info.AppendLine($"{"Temp Directory:",-20} {obj["TempDirectory"]}");
            }
            return info.ToString();
        }

        private static string GetBatteryInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Battery");
            ManagementObjectCollection batteries = searcher.Get();

            StringBuilder info = new StringBuilder();

            if (batteries.Count == 0)
            {
                info.AppendLine("No Win32_Battery instances found (likely a desktop PC).");
            }
            else
            {
                foreach (ManagementObject obj in batteries)
                {
                    info.AppendLine($"{"Device ID:",-25} {obj["DeviceID"]}");
                    info.AppendLine($"{"Design Capacity:",-25} {obj["DesignCapacity"]} mWh");
                    info.AppendLine($"{"Full Charge Capacity:",-25} {obj["FullChargeCapacity"]} mWh");
                    info.AppendLine($"{"Estimated Run Time:",-25} {obj["EstimatedRunTime"]} minutes");
                    //info.AppendLine($"{"Remaining Capacity:",-25} {obj["RemainingCapacity"]} mWh");
                    info.AppendLine($"{"Battery Status Code:",-25} {obj["BatteryStatus"]}");
                }
            }
            return info.ToString();
        }

        /*private static void RenameComputer(string name)  // This will change the device name  // Maybe this should be implemented in another way
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

            object[] newName = { name };
            foreach (ManagementObject obj in searcher.Get())
            {
                obj.InvokeMethod("Rename", newName);
            }
        }*/

        #endregion

        public static void ShowWMI(Operating_Systems OperatingSystems)
        {
            // Layout constants
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0;

            // --- 1. Main Flow Panel for Centering Content ---
            FlowLayoutPanel centerFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                AutoScroll = false
            };

            Panel contentHolder = new Panel
            {
                Size = new Size(PanelWidth, 750),
                Margin = new Padding(0)
            };
            currentY += 32;

            // --- 4. Query Label ---
            Label queryLabel = new Label
            {
                Text = "Choose a query",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Color.FromArgb(230,230,230),
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            contentHolder.Controls.Add(queryLabel);
            currentY += queryLabel.Height + 6;

            // Query Selector
            System.Windows.Forms.ComboBox querySelector = new System.Windows.Forms.ComboBox
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 16, 36),
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Popup,
                DropDownStyle = ComboBoxStyle.DropDownList,

            };
            contentHolder.Controls.Add(querySelector);
            currentY += querySelector.Height + VerticalSpacing;

            // ComboBox elements (Correctly placed outside initializer)
            querySelector.Items.Add("Win32_ComputerSystem (Rename Computer)");
            querySelector.Items.Add("Win32_PerfFormattedData_PerfOS_Memory");
            //querySelector.Items.Add("Win32_LogicalDisk (Partitions)");
            querySelector.Items.Add("Win32_Desktop (Specific info)");
            querySelector.Items.Add("Win32_ComputerSystem (Type)");
            querySelector.Items.Add("Win32_ComputerSystemProduct");
            querySelector.Items.Add("Win32_Desktop (All info)");
            querySelector.Items.Add("Win32_BootConfiguration");
            querySelector.Items.Add("Win32_Service (running)");
            querySelector.Items.Add("Win32_Service (stopped)");
            querySelector.Items.Add("Win32_OperatingSystem");
            querySelector.Items.Add("Win32_ComputerSystem");
            querySelector.Items.Add("Win32_Service (all)");
            querySelector.Items.Add("Win32_LogicalDisk");
            querySelector.Items.Add("Win32_UserAccount");
            querySelector.Items.Add("Win32_CDROMDrive");
            querySelector.Items.Add("Win32_Processor");
            querySelector.Items.Add("Win32_Battery");
            querySelector.Items.Add("Win32_Share");

            if (querySelector.Items.Count > 0) querySelector.SelectedIndex = 0;

            // Content label
            Label contentLabel = new Label
            {
                Text = "Content",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            contentHolder.Controls.Add(contentLabel);
            currentY += contentLabel.Height + 6;

            WmiQueryView resultsPanel = new WmiQueryView
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth - 16, 250),
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 10F),
                BorderStyle = BorderStyle.FixedSingle,
            };
            contentHolder.Controls.Add(resultsPanel);
            currentY += resultsPanel.Height + VerticalSpacing;

            // --- 8. Message / Buttons row (FlowLayoutPanel) ---
            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY), // **Layout Fix: Place FlowPanel using currentY**
                Size = new Size(PanelWidth - 16, 48),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background
            };

            System.Windows.Forms.Button runQueryButton = new System.Windows.Forms.Button
            {
                Text = "Run query",
                Size = new Size(150, 42),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            runQueryButton.FlatAppearance.BorderSize = 0;

            System.Windows.Forms.Button copyButton = new System.Windows.Forms.Button
            {
                Text = "📋 Copy",
                Size = new Size(100, 42),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(10, 0, 0, 0)
            };
            copyButton.FlatAppearance.BorderSize = 0;

            Label messageLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(PanelWidth - 150 - 100 - 60, 40),
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Margin = new Padding(20, 10, 0, 0)
            };

            // Add buttons to the FlowLayoutPanel
            buttonFlow.Controls.Add(runQueryButton);
            buttonFlow.Controls.Add(copyButton);
            buttonFlow.Controls.Add(messageLabel);

            // Button events
            runQueryButton.Click += (s, e) => ShowQuery(resultsPanel, messageLabel, querySelector.SelectedItem.ToString());
            copyButton.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(resultsPanel.Text))
                {
                    Clipboard.SetText(resultsPanel.Text);
                    ShowMessage(messageLabel, "✓ Content copied to clipboard", Operating_Systems.SuccessColor);
                }
            };

            contentHolder.Controls.Add(buttonFlow); // Add control to holder immediately
            currentY += buttonFlow.Height + VerticalSpacing;

            // --- 9. Final Container Adjustments and Placement ---

            contentHolder.Height = currentY;

            centerFlowPanel.Controls.Add(contentHolder);

            OperatingSystems.AddToMainContainer(centerFlowPanel);
        }

        #region UI Helper Methods
        private static void ShowQuery(WmiQueryView resultsPanel, Label messageLabel, string query)
        {
            switch (query)
            {
                case "Win32_ComputerSystem (Rename Computer)":
                    resultsPanel.DisplayResults("Win32_ComputerSystem: Renaming is currently disabled");
                    break;

                case "Win32_PerfFormattedData_PerfOS_Memory":
                    resultsPanel.DisplayResults(GetMemoryInformation());
                    break;

                case "Win32_Desktop (Specific info)":
                    resultsPanel.DisplayResults(GetDesktopInfo());
                    break;

                case "Win32_ComputerSystem (Type)":
                    resultsPanel.DisplayResults(GetComputerType());
                    break;

                case "Win32_ComputerSystemProduct":
                    resultsPanel.DisplayResults(GetProductInfo());
                    break;

                case "Win32_Desktop (All info)":
                    resultsPanel.DisplayResults(GetAllDesktopInfo());
                    break;

                case "Win32_BootConfiguration":
                    resultsPanel.DisplayResults(GetBootConfiguration());
                    break;

                case "Win32_Service 'running'":
                    resultsPanel.DisplayResults(GetServices("running"));
                    break;

                case "Win32_Service 'stopped'":
                    resultsPanel.DisplayResults(GetServices("stopped"));
                    break;

                case "Win32_OperatingSystem":
                    resultsPanel.DisplayResults(Get_OS_Info());
                    break;

                case "Win32_ComputerSystem":
                    resultsPanel.DisplayResults(GetComputerSystemInfo());
                    break;

                case "Win32_Service (all)":
                    resultsPanel.DisplayResults(GetServices());
                    break;

                case "Win32_LogicalDisk":
                    resultsPanel.DisplayResults(GetPartitionsInfo());
                    break;

                case "Win32_UserAccount":
                    resultsPanel.DisplayResults(GetUserAccounts());
                    break;

                case "Win32_CDROMDrive":
                    resultsPanel.DisplayResults(GET_CD_RomInfo());
                    break;

                case "Win32_Processor":
                    resultsPanel.DisplayResults(GetProcessorInfo());
                    break;

                case "Win32_Battery":
                    resultsPanel.DisplayResults(GetBatteryInfo());
                    break;

                case "Win32_Share":
                    resultsPanel.DisplayResults(GetListOfFileShares());
                    break;
            }
        }


        private static void ShowMessage(Label label, string message, Color color)
        {
            label.Text = message;
            label.ForeColor = color;
        }
        #endregion                
    }

    // Custom class made by AI
    public partial class WmiQueryView : UserControl
    {
        private System.Windows.Forms.ListView resultsListView;

        public WmiQueryView()
        {
            //InitializeComponent();

            resultsListView = new System.Windows.Forms.ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                BackColor = Color.FromArgb(43, 43, 43),
                ForeColor = Color.White,
            };
            resultsListView.Columns.Add("Name", 250);
            resultsListView.Columns.Add("Value", 900);

            this.Controls.Add(resultsListView);
        }

        // The method to split a string into lines and parse them as WmiResult objects
        private List<WmiResult> ParseStringToResults(string content)
        {
            var results = new List<WmiResult>();
            if (string.IsNullOrWhiteSpace(content)) return results;

            // Split the content into individual lines using newline and carriage return characters
            var lines = content.Split(
                new[] { '\n', '\r' },
                StringSplitOptions.RemoveEmptyEntries
            );

            foreach (var line in lines)
            {
                // Split the line into a maximum of two parts at the first colon (:)
                string[] parts = line.Split(new[] { ':' }, 2);

                WmiResult result;
                if (parts.Length == 2)
                {
                    // Colon found: Title = part 1, Detail = part 2
                    result = new WmiResult
                    {
                        Title = parts[0].Trim(),
                        Detail = parts[1].Trim()
                    };
                }
                else
                {
                    // No colon found: Title = entire line, Detail = empty
                    result = new WmiResult
                    {
                        Title = line.Trim(),
                        Detail = string.Empty
                    };
                }
                results.Add(result);
            }
            return results;
        }

        // 1. Accepts a single string
        public void DisplayResults(string rawOutput)
        {
            // The entire single string is split into lines, and each line is a result object.
            var results = ParseStringToResults(rawOutput);

            // Display all collected results at once
            DisplayWmiResults(results);
        }

        // 2. Accepts an array of strings
        public void DisplayResults(string[] rawOutputArray)
        {
            var allResults = new List<WmiResult>();

            // Each element in the array is treated as a block of text that needs line splitting
            foreach (var contentBlock in rawOutputArray)
            {
                var blockResults = ParseStringToResults(contentBlock);
                allResults.AddRange(blockResults);
            }

            // Display all collected results from all array elements
            DisplayWmiResults(allResults);
        }

        // Data Binder
        private void DisplayWmiResults(List<WmiResult> wmiResults)
        {
            resultsListView.Items.Clear();
            foreach (var result in wmiResults)
            {
                // Create a new row (ListViewItem)
                var item = new ListViewItem(result.Title);
                // Add the Detail as a sub-item in the second column
                item.SubItems.Add(result.Detail);
                resultsListView.Items.Add(item);
            }
        }
    }
}


namespace Operating_Systems_Project
{
    public class WmiResult
    {
        // الحتة اللي بيكون فيها العنوان، زي 
        // Name, Size, Device ID...
        public string Title { get; set; }

        // المعلومات
        public string Detail { get; set; }
    }
}
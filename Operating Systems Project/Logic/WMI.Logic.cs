using System;
using System.Management;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        /// <summary>
        /// ======= Methods that need to be fixed =======
        /// Win32_Battery (one object doesn't work, the commented one)
        /// Win32_Service (doesn't work when state is set to "running" or "stopped")
        /// Win32_CDROMDrive (doesn't show anything)
        /// </summary>

        private static string[] GetPartitionsInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_LogicalDisk");
            ManagementObjectCollection collection = searcher.Get();

            string[] info = new string[collection.Count];
            int index = 0;

            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;

                info[index] += $"{"Drive Name:",-15} {obj["Name"]}\r\n";
                info[index] += $"{"ID:",-15} {obj["DeviceID"]}\r\n";
                info[index] += $"{"File system:",-15} {obj["FileSystem"]}\r\n";
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

                info[index] += $"{"Name:",-15} {obj["DeviceID"]}\r\n";
                info[index] += $"{"Description:",-15} {obj["Description"]}\r\n";
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

                info[index] += $"{"Name:",-15} {obj["Name"]}\r\n";
                info[index] += $"{"Path:",-15} {obj["Path"]}\r\n";
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

                info[index] += $"{"Service Name:",-15} {obj["DisplayName"]}\r\n";
                info[index] += $"{"Start Mode:",-15} {obj["StartMode"]}\r\n";
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

                info[index] += $"{"Service Name:",-15} {obj["DisplayName"]}\r\n";
                info[index] += $"{"Start Mode:",-15} {obj["StartMode"]}\r\n";
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

                info[index] += $"{"Service Name:",-15} {obj["DisplayName"]}\r\n";
                info[index] += $"{"Start Mode:",-15} {obj["StartMode"]}\r\n";
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

                info[index] += $"{"User name:",-15} {obj["Name"]}\r\n";
                info[index] += $"{"Domain:",-15} {obj["Domain"]}\r\n";
                info[index] += $"{"Status:",-15} {obj["Status"]}\r\n";
                info[index] += $"{"Disabled:",-15} {obj["Disabled"]}\r\n";
                info[index] += $"{"Local account:",-15} {obj["LocalAccount"]}"; // last line — no trailing '\n'

                index++;
            }

            return info;
        }

        private static string[] GetComputerSystemInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            
            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[0] += $"{"Computer Name:",-22} {obj["Name"]}\r\n";
                info[0] += $"{"Domain:",-22} {obj["Domain"]}\r\n";
                info[0] += $"{"Model:",-22} {obj["Model"]}\r\n";
                info[0] += $"{"Manufacturer:",-22} {obj["Manufacturer"]}\r\n";
                info[0] += $"{"Total Physical Memory:",-22} {Math.Round(Convert.ToDouble(obj["TotalPhysicalMemory"]) / (1024.0 * 1024.0 * 1024.0), 2)} GB\n";
                info[0] += $"{"System Type:",-22} {obj["SystemType"]}\r\n";
                info[0] += $"{"Workgroup/Domain Join:",-22} {obj["Workgroup"]}\r\n";
                info[0] += $"{"Type:",-22} {GetComputerType()}";
            }
            return info;
        }

        private static string[] GetComputerType()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            string[] type = new string[1];
            type[0] = "Type: ";
            foreach (ManagementObject obj in searcher.Get())
            {
                switch (Convert.ToInt32(obj["DomainRole"]))
                {
                    case 0:
                        type[0] += "Standalone Workstation";
                        break;
                    case 1:
                        type[0] += "Member Workstation"; // Represents a member of a domain
                        break;
                    case 2:
                        type[0] += "Primary Domain Controller";
                        break;
                    case 3:
                        type[0] += "Backup Domain Controller";
                        break;
                    case 4:
                        type[0] += "Standalone Server"; // Standalone server that's not part of a domain
                        break;
                    case 5:
                        type[0] += "Member Server"; // Server that is a member of a domain
                        break;
                    default:
                        type[0] += "Role Undefined (Code: " + Convert.ToInt32(obj["DomainRole"]) + ")";
                        break;
                }
            }
            return type;
        }

        private static string[] GetProductInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystemProduct");

            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[0] += $"{"Manufacturer:",-20} {obj["Vendor"]}\r\n";
                info[0] += $"{"UUID:",-20} {obj["UUID"]}\r\n";
                info[0] += $"{"Name:",-20} {obj["Name"]}\r\n";
                info[0] += $"{"Identifying Number:",-20} {obj["IdentifyingNumber"]}";
            }
            return info;
        }

        private static string[] GetProcessorInfo()
        {
            ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");

            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in cpuSearcher.Get())
            {
                info[0] += $"Number of Cores: {obj["NumberOfCores"]}";
            }
            return info;
        }

        private static string[] Get_OS_Info()
        {
            ManagementObjectSearcher osSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
            
            string[] info = new string[1];
            info[0] = string.Empty;
            
            foreach (ManagementObject obj in osSearcher.Get())
            {
                info[0] += $"{"Name:",-20}{obj["Caption"]}\n";
                info[0] += $"{"Version:",-20}{obj["Version"]}\n";
                info[0] += $"{"Manufacturer:",-20}{obj["Manufacturer"]}\n";
                info[0] += $"{"Windows Directory:",-20}{obj["WindowsDirectory"]}";
            }
            return info;
        }

        private static string[] GetDesktopInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Desktop WHERE Name = '.Default'");
            
            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[0] += $"{"Desktop Name:",-20} {obj["Name"]}\n";
                info[0] += $"{"Icon Title Size:",-20} {obj["IconTitleSize"]}\n";
                info[0] += $"{"Wallpaper Stretched:",-20} {obj["WallpaperStretched"]}\n";
                info[0] += $"{"Is there a screen saver:",-20} {obj["ScreenSaverActive"]}";

                try
                {
                    if (obj["ScreenSaverActive"].ToString() != "False")
                    {
                        info[0] += $"\n{"Screen Saver time out:",-20} {obj["ScreenSaverTimeout"]}";
                    }
                }
                catch{ }
            }
            return info;
        }

        private static string[] GetAllDesktopInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Desktop WHERE Name = '.Default'");

            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                foreach (PropertyData prop in obj.Properties)
                {
                    info[0] += $"{prop.Name} : {prop.Value}\r\n";
                }
            }
            info[0] = info[0].TrimEnd('\r', '\n');
            return info;
        }

        private static string[] GetMemoryInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PerfFormattedData_PerfOS_Memory");

            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[0] += $"{"Available MBs:",-20} {obj["AvailableMbytes"]}\r\n";
                info[0] += $"{"Cache Bytes:",-20} {obj["CacheBytes"]}\r\n";
                info[0] += $"{"Committed Bytes:",-20} {obj["CommittedBytes"]}\r\n";
                info[0] += $"{"Commit Limit:",-20} {obj["CommitLimit"]}";
            }
            return info;
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

                info[index] += $"{"Description:",-15} {obj["Description"]}\r\n";
                info[index] += $"{"Drive:",-15} {obj["Drive"]}\r\n";
                info[index] += $"{"Media Type:",-15} {obj["MediaType"]}\r\n";
                info[index] += $"{"Size:",-15} {obj["Size"]}\r\n";
                info[index] += $"{"Transfer Rate:",-15} {obj["TransferRate"]}";

                index++;
            }
            return info;
        }

        private static string[] GetBootConfiguration()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BootConfiguration");

            string[] info = new string[1];
            info[0] = string.Empty;

            foreach (ManagementObject obj in searcher.Get())
            {
                info[0] += $"{"BootDirectory:",-20} {obj["BootDirectory"]}\r\n";
                info[0] += $"{"Description:",-20} {obj["Description"]}\r\n";
                info[0] += $"{"Scratch Directory:",-20} {obj["ScratchDirectory"]}\r\n";
                info[0] += $"{"Temp Directory:",-20} {obj["TempDirectory"]}";
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

                info[index] += $"{"Device ID:",-25} {obj["DeviceID"]}\r\n";
                info[index] += $"{"Design Capacity:",-25} {obj["DesignCapacity"]} mWh\r\n";
                info[index] += $"{"Full Charge Capacity:",-25} {obj["FullChargeCapacity"]} mWh\r\n";
                info[index] += $"{"Estimated Run Time:",-25} {obj["EstimatedRunTime"]} minutes\r\n";
                //info[index] += $"{"Remaining Capacity:",-25} {obj["RemainingCapacity"]} mWh\r\n";
                info[index] += $"{"Battery Status Code:",-25} {obj["BatteryStatus"]}";

                index++;
            }
            return info;
        }

        private static string[] RenameComputer()
        {
            //ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");

            //object[] newName = { name };
            //foreach (ManagementObject obj in searcher.Get())
            //{
            //    obj.InvokeMethod("Rename", newName);
            //}
            return new[] { "Win32_ComputerSystem: Renaming is currently disabled" };
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
    }
}
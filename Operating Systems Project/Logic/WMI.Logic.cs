using System;
using System.Management;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        // This method will work with any query, and won't return any empty values
        private static string[] GetQueryInfo(string query)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM {query}");
            ManagementObjectCollection collection = searcher.Get();
            string[] info = new string[collection.Count];
            int index = 0;
            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;
                foreach (PropertyData prop in obj.Properties)
                {
                    // Skip null, empty, or default values
                    if (prop.Value != null &&
                        !string.IsNullOrWhiteSpace(prop.Value.ToString()))
                    {
                        info[index] += $"{prop.Name}: {prop.Value}\r\n";
                    }
                }
                info[index] = info[index].ToString().TrimEnd('\r', '\n');
                index++;
            }
            if (string.IsNullOrEmpty(info[0])) return new[] { "Query didn't return any items" };
            return info;
        }

        // This one will return empty values
        private static string[] GetAllQueryInfo(string query)
        {

            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM {query}");
            ManagementObjectCollection collection = searcher.Get();
            string[] info = new string[collection.Count];
            int index = 0;
            foreach (ManagementObject obj in collection)
            {
                info[index] = string.Empty;
                foreach (PropertyData prop in obj.Properties)
                {
                    info[index] += $"{prop.Name}: {prop.Value}\r\n";
                }
                info[index] = info[index].ToString().TrimEnd('\r', '\n');
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

        private static string[] GetComputerType()
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
            return new[] { type };
        }
    }
}
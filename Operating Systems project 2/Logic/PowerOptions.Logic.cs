using System;
using System.Management;
using System.Runtime.InteropServices;

namespace Operating_Systems_Project
{
    internal partial class PowerOptions
    {
        [DllImport("PowrProf.dll", SetLastError = true)]
        private static extern bool SetSuspendState(bool hibernate, bool forceCritical, bool disableWakeEvent);
        private static void Restart() // Low level Restart using WMI.
        {
            ManagementBaseObject RestartParameters = null;
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");

            os.Scope.Options.EnablePrivileges = true;

            foreach (ManagementObject obj in os.GetInstances())
            {
                RestartParameters = obj.GetMethodParameters("Win32Shutdown");
                // Flags = 6 (Shutdown + Reboot)
                RestartParameters["Flags"] = 6;

                obj.InvokeMethod("Win32Shutdown", RestartParameters, null);
            }
        }
        private static void ShutDown() // Low level ShutDown using WMI.
        {
            ManagementBaseObject ShutdownParameters = null;
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");

            os.Scope.Options.EnablePrivileges = true;

            foreach (ManagementObject obj in os.GetInstances())
            {
                ShutdownParameters = obj.GetMethodParameters("Win32Shutdown");
                // Flags = 2 (Shutdown)
                ShutdownParameters["Flags"] = 2;

                obj.InvokeMethod("Win32Shutdown", ShutdownParameters, null);
            }
        }
        private static void Sleep() // Low level Sleep using WMI.
        {
            ManagementBaseObject SleepParameters = null;
            ManagementClass os = new ManagementClass("Win32_OperatingSystem");

            os.Scope.Options.EnablePrivileges = true;

            foreach (ManagementObject obj in os.GetInstances())
            {
                SleepParameters = obj.GetMethodParameters("Win32Shutdown");
                // Flags = 4 (Suspend/Sleep)
                SleepParameters["Flags"] = 4;

                obj.InvokeMethod("Win32Shutdown", SleepParameters, null);
            }
        }
        public static void Hibernate() // Custom Method for hibernation
        {
            var psi = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = "powercfg",
                Arguments = "/a",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            var output = "";
            using (var p = System.Diagnostics.Process.Start(psi))
            {
                output = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
            }

            if (output.Contains("Hibernate"))
            {
                SetSuspendState(true, false, false);
            }
            else
            {
                throw new NotSupportedException("Hibernation is disabled or unsupported.");
            }
        }
    }
}

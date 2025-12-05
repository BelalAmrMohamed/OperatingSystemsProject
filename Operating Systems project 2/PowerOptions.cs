using System;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class PowerOptions
    {
        public static void ShowPowerOption(Operating_Systems OperatingSystems)
        {            
            // Root panel
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Operating_Systems.Background
            };

            const int PanelWidth = 1110; // 1104
            int currentY = 62;


            // --- Button container ---
            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 100),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background,
                AutoSize = false
            };

            Size buttonSize = new Size(265, 70);

            Button shutdownBtn = CreateStyledButton("Shutdown", buttonSize, Operating_Systems.ErrorColor, ShutDown);
            Button restartBtn = CreateStyledButton("Restart", buttonSize, Operating_Systems.ErrorColor, Restart);
            Button hibernateBtn = CreateStyledButton("Hibernate", buttonSize, Operating_Systems.AccentBlue, Hibernate);
            Button sleepBtn = CreateStyledButton("Sleep", buttonSize, Color.FromArgb(108, 117, 125), Sleep);

            buttonFlow.Controls.Add(shutdownBtn);
            buttonFlow.Controls.Add(restartBtn);
            buttonFlow.Controls.Add(hibernateBtn);
            buttonFlow.Controls.Add(sleepBtn);

            mainPanel.Controls.Add(buttonFlow);
            OperatingSystems.AddToMainContainer(mainPanel);
        }

        public static Button CreateStyledButton(string text, Size size, Color backgroundColor, Action action)
        {
            Color hoverColor = ControlPaint.Dark(backgroundColor, 0.15f);

            Button button = new Button
            {
                Text = text,
                Size = size,
                FlatStyle = FlatStyle.Flat,
                BackColor = backgroundColor,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Cursor = Cursors.Hand,
            };

            button.FlatAppearance.BorderSize = 0;

            button.Click += (s, e) => { AreYouSure.Run(text, action); };

            // --- Hover effect ---
            button.MouseEnter += (s, e) => button.BackColor = hoverColor;
            button.MouseLeave += (s, e) => button.BackColor = backgroundColor;

            return button;
        }


        #region PowerOptions Methods

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

        #endregion

    }
}

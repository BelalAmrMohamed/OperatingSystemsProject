using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class About
    {
        // ---- Constants ----
        // Use a shortened version of app version format. "1.0" instead of "1.0.0", which stand for "MAJOR.MINOR.PATCH"
        private const string APP_VERSION = "2.4";
        private const string PRIMARY_ACCENT_COLOR = "#4A9EFF";

        private static readonly (string Name, string Email)[] DEVELOPERS =
        {
            ("Belal Amr Mohamed", "belalamrofficial@gmail.com"),
            ("Mohamed Ahmed Tawfeeq", "mohamedahmed@gmail.com"),
            ("Ahmed Mohamed Husaini", "ahmedmohamed@gmail.com"),
            ("Mahmoud Gad Alkareem", "MahmoudGad@gmail.com"),
            ("Ahmed Khairy Ahmed", "AhmedKhairy@gmail.com"),
            ("Abdulra'of Mohamed Abdulra'of", "AbdulraofMohamed@gmail.com"),
        };

        public static void ShowAbout(Operating_Systems OperatingSystems)
        {
            // Define Layout Constants
            int contentWidth = (int)(Operating_Systems.formWidth * 0.95);
            int rowHeight = 55;
            Color cardColor = Operating_Systems.Background;

            FlowLayoutPanel mainFlow = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Operating_Systems.Background,
                AutoScroll = false
            };

            // --- Content Panel ---
            Panel contentPanel = new Panel
            {
                Width = contentWidth,
                BackColor = cardColor,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
            };

            // keep content centered when mainFlow resizes
            void RecenterContent()
            {
                int left = Math.Max(0, (mainFlow.ClientSize.Width - contentPanel.Width) / 2);
            }
            mainFlow.Resize += (s, e) => RecenterContent();

            int currentY = 40;

            // --- 1. Header Section (Logo + Title + Top Buttons) ---

            // Logo box
            Panel logoBox = new Panel
            {
                Size = new Size(60, 60),
                BackColor = Operating_Systems.PanelColor,
                Location = new Point(20, currentY),
                BorderStyle = BorderStyle.FixedSingle,
            };

            PictureBox pb = new PictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom
            };

            Label initials = new Label
            {
                Text = "OS",
                Font = new Font("Segoe UI Semibold", 18F),
                ForeColor = Color.White,
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };
            logoBox.Controls.Add(initials);
            contentPanel.Controls.Add(logoBox);

            // Title
            Label appName = new Label
            {
                Text = "Operating Systems App",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(90, currentY)
            };
            contentPanel.Controls.Add(appName);

            // Version
            Label versionLabel = new Label
            {
                Text = $"Version {APP_VERSION}",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Location = new Point(92, currentY + 32)
            };
            contentPanel.Controls.Add(versionLabel);

            // Top Right Buttons (Fixed Widths to prevent "Check..." cutoff)
            Button btnUpdate = CreateLinkButton("Check Updates", new Point(contentWidth - 140, currentY + 10));
            btnUpdate.Size = new Size(110, 30); // Explicit larger size

            Button btnCopyVer = CreateLinkButton("Copy Version", new Point(contentWidth - 250, currentY + 10));
            btnCopyVer.Size = new Size(100, 30);

            // Events
            btnCopyVer.Click += (s, e) => { Clipboard.SetText(APP_VERSION); MessageBox.Show("Version copied."); };
            btnUpdate.Click += (s, e) => MessageBox.Show("You are on the latest version.");

            contentPanel.Controls.Add(btnUpdate);
            contentPanel.Controls.Add(btnCopyVer);

            currentY += 80; // Move down past header

            // --- 2. Description Section ---
            Label infoLabel = new Label
            {
                Text = "A robust utility for File I/O operations, Real-time Folder Monitoring, Precision Stopwatch, WMI System Queries, and advanced Power Management control.",
                Font = new Font("Segoe UI", 11F), // Slightly larger font
                ForeColor = Color.FromArgb(200, 200, 200),
                MaximumSize = new Size(contentWidth - 40, 0),
                AutoSize = true,
                Location = new Point(20, currentY)
            };
            contentPanel.Controls.Add(infoLabel);
            currentY += infoLabel.Height + 20;

            // Divider Line
            Panel divider = new Panel { Height = 1, Width = contentWidth - 40, BackColor = Color.FromArgb(60, 60, 60), Location = new Point(20, currentY) };
            contentPanel.Controls.Add(divider);
            currentY += 20;

            currentY -= 7;

            // DEVELOPERS collection expected to be available (your code used DEVELOPERS previously)
            // If you don't have a DEVELOPERS structure / list, adapt this loop to use DEVELOPERS_NAMES / DEVELOPER_EMAILS
            foreach (var dev in DEVELOPERS)
            {
                Panel row = new Panel
                {
                    Size = new Size(contentWidth - 40, rowHeight),
                    Location = new Point(20, currentY),
                    BackColor = Color.Transparent // Or alternating colors if preferred
                };

                // NAME (Left)
                Label lblName = new Label
                {
                    Text = dev.Name,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Location = new Point(0, 15), // Vertically centered roughly
                    AutoSize = true
                };

                // EMAIL (Middle - Fixed position to align columns neatly)
                Label lblEmail = new Label
                {
                    Text = dev.Email,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.Gray,
                    Location = new Point(250, 15), // Fixed X position creates a clean column
                    AutoSize = true
                };

                // BUTTONS (Right Aligned)
                int btnY = 12;

                // Contact Button
                Button btnContact = CreateLinkButton("Contact", new Point(row.Width - 90, btnY));
                btnContact.Size = new Size(80, 30); // Ensure text fits
                btnContact.Click += (s, e) =>
                {
                    try
                    {
                        var psi = new ProcessStartInfo($"mailto:{dev.Email}") { UseShellExecute = true };
                        Process.Start(psi);
                    }
                    catch
                    {
                        Clipboard.SetText(dev.Email);
                        MessageBox.Show("Email copied.");
                    }
                };

                // Copy Button
                Button btnCopy = CreateLinkButton("Copy", new Point(row.Width - 180, btnY));
                btnCopy.Size = new Size(80, 30);
                btnCopy.Click += (s, e) =>
                {
                    Clipboard.SetText(dev.Email);
                    MessageBox.Show("Email copied.");
                };

                row.Controls.Add(lblName);
                row.Controls.Add(lblEmail);
                row.Controls.Add(btnContact);
                row.Controls.Add(btnCopy);

                // Add nice hover effect for the row
                row.MouseEnter += (s, e) => row.BackColor = Color.FromArgb(35, 35, 35);
                row.MouseLeave += (s, e) => row.BackColor = Color.Transparent;

                contentPanel.Controls.Add(row);
                currentY += rowHeight;
            }

            // --- 4. Footer ---
            currentY += 20;
            Label footer = new Label
            {
                Text = $"© {DateTime.Now.Year} OperatingSystemsApp Team. All rights reserved.",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.FromArgb(80, 80, 80),
                AutoSize = true,
                Location = new Point(20, currentY)
            };
            contentPanel.Controls.Add(footer);

            // Initial recenter, add content and attach resize handler
            RecenterContent();
            mainFlow.Controls.Add(contentPanel);
            OperatingSystems.AddToMainContainer(mainFlow);

        }


        private static Button CreateLinkButton(string text, Point loc)
        {
            return new Button
            {
                Text = text,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(40, 40, 40) },
                BackColor = Color.FromArgb(30, 32, 34), // Slight background to make them pop
                ForeColor = ColorTranslator.FromHtml(PRIMARY_ACCENT_COLOR),
                Font = new Font("Segoe UI", 9F),
                Location = loc,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
        }
    }
}
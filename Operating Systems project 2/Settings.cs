using System;
using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class Settings
    {
        public static Label headerLabel;
        public static Label sectionLabel;
        public static Panel themePanel;
        public static Label lblTheme;
        public static Button btnToggleTheme;

        public static Panel BackgroundImagePanel;
        public static Label lblBackgroundImage;
        public static Button btnToggleBackgroundImage;
        public static void ShowSettings(Operating_Systems OS)
        {
            // Define Layout Constants
            int contentWidth = (int)(Operating_Systems.formWidth * 0.95);
            int currentY = 30;

            // 1. Header Title
            headerLabel = new Label
            {
                Text = "Settings (Experimental)",
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(20, currentY)
            };
            OS.AddToMainContainer(headerLabel);
            currentY += 60;

            // 2. Section: Personalization
            sectionLabel = new Label
            {
                Text = "Personalization",
                Font = new Font("Segoe UI", 14F, FontStyle.Regular),
                ForeColor = Operating_Systems.AccentBlue,
                AutoSize = true,
                Location = new Point(20, currentY)
            };
            OS.AddToMainContainer(sectionLabel);
            currentY += 40;

            // Container for the Theme Option
            themePanel = new Panel
            {
                Size = new Size(contentWidth - 40, 60),
                Location = new Point(20, currentY),
                Padding = new Padding(10)
            };

            // Label: "App Theme"
            lblTheme = new Label
            {
                Text = "App Theme Mode",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(10, 15),
                AutoSize = true
            };

            // Button: Toggle Switch
            btnToggleTheme = new Button
            {
                Text = IsDarkMode() ? "Switch to Light Mode ☀" : "Switch to Dark Mode 🌙",
                Font = new Font("Segoe UI Semibold", 9F),
                Size = new Size(160, 35),
                Location = new Point(themePanel.Width - 170, 12),
                FlatStyle = FlatStyle.Flat,
                BackColor = IsDarkMode() ? Color.FromArgb(60, 60, 60) : Color.FromArgb(220, 220, 220),
                ForeColor = IsDarkMode() ? Color.White : Color.Black,
                Cursor = Cursors.Hand
            };
            btnToggleTheme.FlatAppearance.BorderSize = 0;

            // --- Toggle Logic ---
            btnToggleTheme.Click += (s, e) =>
            {
                if (IsDarkMode())
                {
                    Colors.ApplyLightMode();
                    Operating_Systems.RefreshBackgroundColor(OS);
                }
                else
                {
                    Colors.ApplyDarkMode();
                    Operating_Systems.RefreshBackgroundColor(OS);
                }

                Colors.RefreshSettingsColors();
            };

            themePanel.Controls.Add(lblTheme);
            themePanel.Controls.Add(btnToggleTheme);
            OS.AddToMainContainer(themePanel);

            currentY += 80;

            // Divider
            Panel divider = new Panel
            {
                Height = 1,
                Width = contentWidth - 40,
                BackColor = Operating_Systems.TextSecondary,
                Location = new Point(20, currentY)
            };
            OS.AddToMainContainer(divider);

            currentY += 20;

            // Container for the Background image Option
            BackgroundImagePanel = new Panel
            {
                Size = new Size(contentWidth - 40, 60),
                Location = new Point(20, currentY),
                Padding = new Padding(10)
            };

            // Label: "Background image"
            lblBackgroundImage = new Label
            {
                Text = "Background image",
                Font = new Font("Segoe UI", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                Location = new Point(10, 15),
                AutoSize = true
            };

            // Button: Toggle Switch
            btnToggleBackgroundImage = new Button
            {
                Text = OS.IsBackgroundImageEnabled() ? "Remove background" : "Add Background image",
                Font = new Font("Segoe UI Semibold", 9F),
                Size = new Size(160, 35),
                Location = new Point(BackgroundImagePanel.Width - 170, 12),
                FlatStyle = FlatStyle.Flat,
                BackColor = btnToggleTheme.BackColor,
                ForeColor = btnToggleTheme.ForeColor,
                Cursor = Cursors.Hand
            };
            btnToggleBackgroundImage.FlatAppearance.BorderSize = 0;

            btnToggleBackgroundImage.Click += (s, e) =>
            {
                if (OS.IsBackgroundImageEnabled())
                {
                    OS.ToggleBackgroundImage();
                    btnToggleBackgroundImage.Text = "Add Background";                    
                }
                else
                {
                    OS.ToggleBackgroundImage(true);
                    btnToggleBackgroundImage.Text = "Remove background";
                }
            };

            BackgroundImagePanel.Controls.Add(lblBackgroundImage);
            BackgroundImagePanel.Controls.Add(btnToggleBackgroundImage);
            OS.AddToMainContainer(BackgroundImagePanel);

            currentY += 80;

            // Divider 2
            Panel divider_2 = new Panel
            {
                Height = 1,
                Width = contentWidth - 40,
                BackColor = Operating_Systems.TextSecondary,
                Location = new Point(20, currentY)
            };
            OS.AddToMainContainer(divider_2);
        }

        // Helper to check current state (Checking Background color in a simple way)
        public static bool IsDarkMode()
        {
            // Checks if the background is the dark color (32, 32, 32)
            return Operating_Systems.Background.R == 32;
        }
    }
    public class Colors
    {
        public static void RefreshSettingsColors()
        {
            Settings.headerLabel.ForeColor = Operating_Systems.TextPrimary;
            Settings.sectionLabel.ForeColor = Operating_Systems.AccentBlue;
            Settings.lblTheme.ForeColor = Operating_Systems.TextPrimary;
            Settings.btnToggleTheme.BackColor = Settings.IsDarkMode() ? Color.FromArgb(60, 60, 60) : Color.FromArgb(220, 220, 220);
            Settings.btnToggleTheme.ForeColor = Settings.IsDarkMode() ? Color.White : Color.Black;
            Settings.btnToggleBackgroundImage.BackColor = Settings.btnToggleTheme.BackColor;
            Settings.btnToggleBackgroundImage.ForeColor = Settings.btnToggleTheme.ForeColor;
        }
        public static void ApplyDarkMode()
        {
            Settings.btnToggleTheme.Text = "Switch to Light Mode ☀";
            Settings.btnToggleTheme.BackColor = Color.FromArgb(60, 60, 60);
            Settings.btnToggleTheme.ForeColor = Color.White;

            Operating_Systems.Background = Color.FromArgb(32, 32, 32);
            Operating_Systems.PanelColor = Color.FromArgb(43, 43, 43);
            Operating_Systems.WMISmallPanelColor = Color.FromArgb(230, 230, 130);
            Operating_Systems.TextPrimary = Color.FromArgb(255, 255, 255);
            Operating_Systems.TextSecondary = Color.FromArgb(160, 160, 160);
            Operating_Systems.NeutralColor = Color.FromArgb(108, 117, 125);
            Operating_Systems.DarkButton = Color.FromArgb(30, 32, 34);
            Operating_Systems.ButtonsPanelBackground = Color.FromArgb(25, 25, 25);
            // Accents usually stay the same, but you can adjust if needed
        }

        public static void ApplyLightMode()
        {
            Settings.btnToggleTheme.Text = "Switch to Dark Mode 🌙";
            Settings.btnToggleTheme.BackColor = Color.FromArgb(220, 220, 220);
            Settings.btnToggleTheme.ForeColor = Color.Black;

            // Define your Light Mode Palette here
            Operating_Systems.Background = Color.FromArgb(245, 245, 245);       // Light Gray/White
            Operating_Systems.PanelColor = Color.FromArgb(255, 255, 255);       // Pure White panels
            Operating_Systems.WMISmallPanelColor = Color.FromArgb(0, 30, 50);
            Operating_Systems.TextPrimary = Color.FromArgb(30, 30, 30);         // Dark Text
            Operating_Systems.TextSecondary = Color.FromArgb(90, 90, 90);       // Gray Text
            Operating_Systems.NeutralColor = Color.FromArgb(200, 200, 200);     // Light Borders/Neutrals
            Operating_Systems.DarkButton = Color.FromArgb(225, 225, 225);     // Gray
            Operating_Systems.ButtonsPanelBackground = Color.FromArgb(255, 255, 255);
        }
    }
}
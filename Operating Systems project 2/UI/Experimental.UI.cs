using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class Experimental
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
                    ApplyLightMode(OS);
                    Operating_Systems.RefreshBackgroundColor(OS);
                }
                else
                {
                    ApplyDarkMode(OS);
                    Operating_Systems.RefreshBackgroundColor(OS);
                }

                RefreshSettingsColors();
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
                    OS.ToggleBackgroundImage();
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
    }
}
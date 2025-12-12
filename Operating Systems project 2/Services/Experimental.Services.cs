using System.Drawing;

namespace Operating_Systems_Project
{
    internal partial class Experimental
    {
        public static bool IsDarkMode()
        {
            // Checks if the background is the dark color (32, 32, 32)
            return Operating_Systems.Background.R == 32;
        }

        private static void RefreshSettingsColors()
        {
            headerLabel.ForeColor = Operating_Systems.TextPrimary;
            sectionLabel.ForeColor = Operating_Systems.AccentBlue;
            lblTheme.ForeColor = Operating_Systems.TextPrimary;
            lblBackgroundImage.ForeColor = Operating_Systems.TextPrimary;
            btnToggleTheme.BackColor = IsDarkMode() ? Color.FromArgb(60, 60, 60) : Color.FromArgb(220, 220, 220);
            btnToggleTheme.ForeColor = IsDarkMode() ? Color.White : Color.Black;
            btnToggleBackgroundImage.BackColor = btnToggleTheme.BackColor;
            btnToggleBackgroundImage.ForeColor = btnToggleTheme.ForeColor;
        }

        private static void ApplyDarkMode()
        {
            btnToggleTheme.Text = "Switch to Light Mode ☀";
            btnToggleTheme.BackColor = Color.FromArgb(60, 60, 60);
            btnToggleTheme.ForeColor = Color.White;

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

        private static void ApplyLightMode()
        {
            btnToggleTheme.Text = "Switch to Dark Mode 🌙";
            btnToggleTheme.BackColor = Color.FromArgb(220, 220, 220);
            btnToggleTheme.ForeColor = Color.Black;

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

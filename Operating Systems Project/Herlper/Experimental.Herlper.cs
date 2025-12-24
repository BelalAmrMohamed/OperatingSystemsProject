using System.Drawing;

namespace Operating_Systems_Project
{
    internal partial class Experimental
    {
        private static void RefreshSettingsColors()
        {
            headerLabel.ForeColor = Operating_Systems.TextPrimary;
            sectionLabel.ForeColor = Operating_Systems.AccentBlue;
            lblTheme.ForeColor = Operating_Systems.TextPrimary;
            lblBackgroundImage.ForeColor = Operating_Systems.TextPrimary;
            btnToggleTheme.BackColor = Operating_Systems.IsDarkMode() ? Color.FromArgb(60, 60, 60) : Color.FromArgb(220, 220, 220);
            btnToggleTheme.ForeColor = Operating_Systems.IsDarkMode() ? Color.White : Color.Black;
            btnToggleBackgroundImage.BackColor = btnToggleTheme.BackColor;
            btnToggleBackgroundImage.ForeColor = btnToggleTheme.ForeColor;
        }

        public static void ApplyDarkMode(Operating_Systems OS)
        {
            btnToggleTheme.Text = "Switch to Light Mode ☀";
            btnToggleTheme.BackColor = Color.FromArgb(60, 60, 60);
            btnToggleTheme.ForeColor = Color.White;

            OS.DarkTitleBar(true);

            Operating_Systems.Background = Color.FromArgb(32, 32, 32);
            Operating_Systems.PanelColor = Color.FromArgb(43, 43, 43);
            WMI.SmallPanelColor = Color.FromArgb(0, 30, 50);
            Operating_Systems.TextPrimary = Color.FromArgb(255, 255, 255);
            Operating_Systems.TextSecondary = Color.FromArgb(160, 160, 160);
            Operating_Systems.NeutralColor = Color.FromArgb(108, 117, 125);
            Operating_Systems.DarkButton = Color.FromArgb(30, 32, 34);
            Operating_Systems.ButtonsPanelBackground = Color.FromArgb(25, 25, 25);

            RefreshSettingsColors();
            Operating_Systems.RefreshBackgroundColor(OS); 
        }

        public static void ApplyLightMode(Operating_Systems OS)
        {
            btnToggleTheme.Text = "Switch to Dark Mode 🌙";
            btnToggleTheme.BackColor = Color.FromArgb(220, 220, 220);
            btnToggleTheme.ForeColor = Color.Black;

            OS.DarkTitleBar(false);

            // Define your Light Mode Palette here
            Operating_Systems.Background = Color.FromArgb(245, 245, 245);       // Light Gray/White
            Operating_Systems.PanelColor = Color.FromArgb(255, 255, 255);       // Pure White panels
            WMI.SmallPanelColor = Color.FromArgb(240, 240, 240);
            Operating_Systems.TextPrimary = Color.FromArgb(30, 30, 30);         // Dark Text
            Operating_Systems.TextSecondary = Color.FromArgb(90, 90, 90);       // Gray Text
            Operating_Systems.NeutralColor = Color.FromArgb(200, 200, 200);     // Light Borders/Neutrals
            Operating_Systems.DarkButton = Color.FromArgb(225, 225, 225);     // Gray
            Operating_Systems.ButtonsPanelBackground = Color.FromArgb(255, 255, 255);

            RefreshSettingsColors();
            Operating_Systems.RefreshBackgroundColor(OS);
        }
    }
}
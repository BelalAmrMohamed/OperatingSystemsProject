using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        public static Color SmallPanelColor = Color.FromArgb(0, 30, 50);       

        private static void ShowQuery_MultiTextBoxes(string[] info)
        {
            const int containerWidth = 1103;
            const int verticalSpacing = 3;
            int currentY = verticalSpacing;

            foreach (string text in info)
            {
                var resultsBox = new Label
                {
                    Font = new Font("Segoe UI Semibold", 11F),
                    ForeColor = Operating_Systems.TextPrimary,
                    BackColor = SmallPanelColor, // 1- 59, 60, 109 // 2- AccentBlue // 3- White and black // 4- 108, 250, 125 // 5- 0, 30, 50
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(30, 30, 30, 30),
                    Width = containerWidth - 25,
                    Location = new Point(3, currentY),
                    Text = text
                };

                resultsBox.Height = GetBoxHeight(text);

                resultsBox.DoubleClick += (s, e) =>
                {
                    Clipboard.SetText(resultsBox.Text);
                    _ = Display_CopiedSuccessfully(); // The '_ = ' is a fix to the restricted 'await' problem
                };

                resultsPanel.Controls.Add(resultsBox);
                currentY += resultsBox.Height + verticalSpacing;
            }
            Panel ExtraSpaceAtTheBottom = new Panel
            {
                Size = new Size(resultsPanel.Width - 100, verticalSpacing),
                Location = new Point(0, currentY - verticalSpacing),
            };
            resultsPanel.Controls.Add(ExtraSpaceAtTheBottom);
        }

        /// ======================
        /// Helper Methods
        /// ======================

        private static int GetBoxHeight(string text)
        {
            // The height is: number of lines * 21.
            // 21 is one line's height.
            // 425 is the maximum height.
            return Math.Min(text.Split('\n').Length * 21, 425);
        }

        private static async Task Display_CopiedSuccessfully()
        {
            copyMessageLabel.Text = SuccessfulCopyingMessage;
            copyMessageLabel.ForeColor = Color.Green;

            await Task.Delay(2000); // Non-blocking wait for 3 seconds

            copyMessageLabel.Text = DefaultCopyMessage;
            copyMessageLabel.ForeColor = Operating_Systems.TextPrimary;
        }
    }
}
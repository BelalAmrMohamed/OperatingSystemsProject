using System.Drawing;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class FileReader
    {
        public static void ShowFileReader(Operating_Systems OS)
        {
            // Layout constants (match FileWriter)
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0;

            Label HeaderLabel = new Label
            {
                Text = "📖 File Reader",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                MinimumSize = new Size(200, 30),
                Location = new Point(457, 8),
                Size = new Size(200, 30),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.HeaderColor,
            };
            Label SubHeaderLabel = new Label
            {
                Text = "Read and display content from text or binary files.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                MinimumSize = new Size(300, 20),
                Location = new Point(414, 38),
                Size = new Size(300, 20),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Operating_Systems.TextPrimary,
            };
            currentY += 32;

            // File Path label
            Label labelInputPath = new Label
            {
                Text = "Path",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += labelInputPath.Height + 6;

            // Path Panel
            Panel pathPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 36),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8, 4, 8, 4)
            };

            Button browseButton = new Button
            {
                Text = "Browse",
                Size = new Size(90, 28),
                Location = new Point(PanelWidth - 95, 3),
                BackColor = Operating_Systems.AccentGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand
            };
            browseButton.FlatAppearance.BorderSize = 0;

            TextBox pathTextBox = new TextBox
            {
                Name = "pathTextBox",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = pathPanel.BackColor,
                BorderStyle = BorderStyle.None,
                Location = new Point(8, 8),
                Size = new Size(PanelWidth - browseButton.Width - 14, 25),
                Text = string.Empty
            };

            browseButton.Click += (s, e) => BrowseForFile(pathTextBox);

            pathPanel.Controls.Add(pathTextBox);
            pathPanel.Controls.Add(browseButton);
            currentY += 36 + VerticalSpacing;

            // File Info (hidden initially) - styled like FileWriter (dark theme)
            Panel fileInfoPanel = new Panel
            {
                Location = new Point(0, currentY - 10),
                Size = new Size(PanelWidth, 36),
                BackColor = Color.Transparent,
                BorderStyle = BorderStyle.None,
                Visible = false
            };
            Label fileInfoLabel = new Label
            {
                Name = "fileInfoLabel",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Location = new Point(0, 8)
            };
            fileInfoPanel.Controls.Add(fileInfoLabel);
            currentY += fileInfoLabel.Height - 15;

            // Content label
            Label contentLabel = new Label
            {
                Text = "Content",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += contentLabel.Height + 6;

            // Content panel and textbox (styled to match FileWriter)
            const int ContentPanelHeight = 250;
            Panel contentPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, ContentPanelHeight - 30),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };

            TextBox contentTextBox = new TextBox
            {
                Name = "contentTextBox",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = Operating_Systems.PanelColor,
                Multiline = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.None,
                //WordWrap = false,
                Size = new Size(PanelWidth, ContentPanelHeight - 46), // Fill content panel
                Location = new Point(8, 8),
                ReadOnly = true,
                Text = "No file loaded. Click 'Read File' to load content."
            };

            contentPanel.Controls.Add(contentTextBox);
            currentY += ContentPanelHeight + VerticalSpacing - 15;

            // Message / Buttons row (FlowLayoutPanel like FileWriter)
            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 48),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
            };

            Button readButton = new Button
            {
                Text = "📖 Read File",
                Size = new Size(150, 42),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            readButton.FlatAppearance.BorderSize = 0;

            Button copyButton = new Button
            {
                Text = "📋 Copy",
                Size = new Size(100, 42),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Enabled = false,
                Margin = new Padding(10, 0, 0, 0)
            };
            copyButton.FlatAppearance.BorderSize = 0;

            Button clearButton = new Button
            {
                Text = "Clear",
                Size = new Size(100, 42),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F),
                Cursor = Cursors.Hand,
                Margin = new Padding(10, 0, 0, 0)
            };
            clearButton.FlatAppearance.BorderSize = 0;

            Label messageLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(PanelWidth - 150 - 100 - 60, 40),
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Margin = new Padding(20, 10, 0, 0)
            };

            buttonFlow.Controls.Add(readButton);
            buttonFlow.Controls.Add(copyButton);
            buttonFlow.Controls.Add(clearButton);
            buttonFlow.Controls.Add(messageLabel);

            // Attach button events
            readButton.Click += (s, e) => ReadFile(pathTextBox, contentTextBox, messageLabel, fileInfoLabel, fileInfoPanel, copyButton);
            copyButton.Click += (s, e) =>
            {
                if (!string.IsNullOrEmpty(contentTextBox.Text))
                {
                    Clipboard.SetText(contentTextBox.Text);
                    ShowMessage(messageLabel, "✓ Content copied to clipboard", Operating_Systems.SuccessColor);
                }
            };
            clearButton.Click += (s, e) =>
            {
                contentTextBox.Text = "No file loaded. Click 'Read File' to load content.";
                messageLabel.Text = string.Empty;
                fileInfoPanel.Visible = false;
                copyButton.Enabled = false;
                contentTextBox.BackColor = Operating_Systems.PanelColor;
                contentTextBox.ForeColor = Operating_Systems.TextPrimary;
            };

            OS.AddToMainContainer(HeaderLabel);
            OS.AddToMainContainer(SubHeaderLabel);
            OS.AddToMainContainer(labelInputPath);
            OS.AddToMainContainer(pathPanel);
            OS.AddToMainContainer(fileInfoPanel);
            OS.AddToMainContainer(contentLabel);
            OS.AddToMainContainer(contentPanel);
            OS.AddToMainContainer(buttonFlow);
            pathTextBox.Focus();
        }
    }
}

using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class FileReader
    {
        #region 2 Reading Methods, for Text and Binary
        private static string ReadTextFile(string path)
        {
            using (FileStream fileS = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BufferedStream buffer = new BufferedStream(fileS))
            using (StreamReader reader = new StreamReader(buffer, Encoding.UTF8, true))
                return reader.ReadToEnd();
        }

        private static string ReadBinaryFile(string path) // Returns text
        {
            byte[] bytes;

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))            
               bytes = reader.ReadBytes((int)reader.BaseStream.Length);            

            return Encoding.UTF8.GetString(bytes);
        }

        private static string ReadBinaryFileAsHexadecimal(string path) // Returns Hexadecimal
        {
            byte[] bytes = File.ReadAllBytes(path);

            StringBuilder hex = new StringBuilder(bytes.Length * 3);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:X2} ", b);

            return hex.ToString().Trim(); // Remove last space
        }


        #endregion

        private static void ReadFile(TextBox pathTextBox, TextBox contentTextBox, Label messageLabel, Label fileInfoLabel, Panel fileInfoPanel, Button copyButton)
        {
            string path = pathTextBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(path))
            {
                ShowMessage(messageLabel, "⚠ Please enter a file path.", Operating_Systems.ErrorColor);
                return;
            }

            if (!File.Exists(path))
            {
                ShowMessage(messageLabel, "⚠ File does not exist.", Operating_Systems.ErrorColor);
                contentTextBox.Text = "File not found.";
                fileInfoPanel.Visible = false;
                return;
            }

            try
            {
                string extension = Path.GetExtension(path).ToLower();                
                StringBuilder content = new StringBuilder();

                if (extension == ".bin") 
                {
                    content.AppendLine("Binary File Content (Hexadecimal):");
                    content.AppendLine(ReadBinaryFileAsHexadecimal(path));
                    content.AppendLine(new string('=', 109));
                    content.AppendLine();
                    content.AppendLine("Binary File Content (Text):");
                    content.Append(ReadBinaryFile(path));
                }
                else content.Append(ReadTextFile(path));
                
                contentTextBox.Text = content.ToString();

                FileInfo fileInfo = new FileInfo(path);
                fileInfoLabel.Text = $"📄 {Path.GetFileName(path)} | Size: {FormatFileSize(fileInfo.Length)} | Modified: {fileInfo.LastWriteTime:yyyy-MM-dd HH:mm:ss}";
                fileInfoPanel.Visible = true;

                copyButton.Enabled = true;

                ShowMessage(messageLabel, $"✓ Successfully read file ({content.Length} characters)", Operating_Systems.SuccessColor);
            }
            catch (Exception ex)
            {
                ShowMessage(messageLabel, $"✗ Error: {ex.Message}", Operating_Systems.ErrorColor);
                contentTextBox.Text = $"Error reading file:\n{ex.Message}";
                fileInfoPanel.Visible = false;
            }
        }

        private static string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        public static void ShowFileReader(Operating_Systems OperatingSystems)
        {
            // Layout constants (match FileWriter)
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0;

            // Main Flow Panel for Centering Content (same approach as FileWriter)
            FlowLayoutPanel centerFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Padding = new Padding(0),
                BackColor = Operating_Systems.Background,
                AutoScroll = false
            };

            // Main content holder (fixed width)
            Panel contentHolder = new Panel
            {
                Size = new Size(PanelWidth, 500),
                BackColor = Operating_Systems.Background,
                Margin = new Padding(0)
            };

            Label HeaderLabel = new Label
            {
                Text = "📖 File Reader",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                MinimumSize = new Size(200, 30),
                Location = new Point(457, 8),
                Size = new Size(200, 30),

                AutoSize = true,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(230, 230, 130),
                BackColor = Color.FromArgb(31, 31, 31),
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
                ForeColor = Color.FromArgb(230, 230, 230),
                BackColor = Color.FromArgb(31, 31, 31),
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
                BackColor = Operating_Systems.AccentBlue,
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
                BackColor = Operating_Systems.PanelColor,
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
                BackColor = Operating_Systems.Background
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

            // Add controls to holder
            contentHolder.Controls.AddRange(new Control[]
            {
                HeaderLabel,
                SubHeaderLabel,
                labelInputPath,
                pathPanel,
                fileInfoPanel,
                contentLabel,
                contentPanel,
                buttonFlow
            });

            centerFlowPanel.Controls.Add(contentHolder);
            OperatingSystems.AddToMainContainer(centerFlowPanel);

            pathTextBox.Focus();
        }

        private static void BrowseForFile(TextBox pathTextBox)
        {
            using (OpenFileDialog openDialog = new OpenFileDialog())
            {
                openDialog.Filter = "Text Files (*.txt)|*.txt|Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
                openDialog.Title = "Select File to Read";

                if (!string.IsNullOrEmpty(pathTextBox.Text))
                {
                    try
                    {
                        openDialog.InitialDirectory = Path.GetDirectoryName(pathTextBox.Text);
                        openDialog.FileName = Path.GetFileName(pathTextBox.Text);
                    }
                    catch { }
                }

                if (openDialog.ShowDialog() == DialogResult.OK)
                {
                    pathTextBox.Text = openDialog.FileName;
                }
            }
        }

        private static void ShowMessage(Label label, string message, Color color)
        {
            label.Text = message;
            label.ForeColor = color;
        }
    }
}

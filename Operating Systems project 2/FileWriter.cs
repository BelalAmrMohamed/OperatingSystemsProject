using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal class FileWriter
    {

#region 2 Writing Methods, for Text and Binary
        private static void WriteToTextFile(string path, string content)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using (FileStream fileS = new FileStream(path, FileMode.Append))
                using (BufferedStream buffer = new BufferedStream(fileS))
                using (StreamWriter writer = new StreamWriter(buffer))
                    writer.WriteLine(content);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to write text file: {ex.Message}", ex);
            }
        }

        private static void WriteToBinaryFile(string path, string content)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                // This is the part that we learned at the section
                using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
                    writer.Write(content);
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to write binary file: {ex.Message}", ex);
            }
        }
        #endregion        

        private static void WriteFile(TextBox pathTextBox, TextBox contentTextBox, Label messageLabel)
        {
            string path = pathTextBox.Text.Trim();
            string content = contentTextBox.Text;

            if (string.IsNullOrWhiteSpace(path))
            {
                ShowMessage(messageLabel, "⚠ Please enter a file path.", Operating_Systems.ErrorColor);
                return;
            }

            if (string.IsNullOrEmpty(content))
            {
                ShowMessage(messageLabel, "⚠ Content cannot be empty.", Operating_Systems.ErrorColor);
                return;
            }

            try
            {
                string filetype = Path.GetExtension(path).ToLower();

                if (filetype == ".txt")
                    WriteToTextFile(path, content);

                else // binary
                    WriteToBinaryFile(path, content);


                long size = new FileInfo(path).Length;

                ShowMessage(messageLabel,
                    $"✓ Successfully wrote to '{Path.GetFileName(path)}'  ({size} bytes)",
                    Operating_Systems.SuccessColor);
            }
            catch (Exception ex)
            {
                ShowMessage(messageLabel, $"✗ Error: {ex.Message}", Operating_Systems.ErrorColor);
            }
        }

        public static void ShowFileWriter(Operating_Systems OperatingSystems)
        {
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0; // Tracks vertical position            

            // Main Flow Panel for Centering Content
            // We use a FlowLayoutPanel here to easily center a single wide panel.
            FlowLayoutPanel centerFlowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                // Align content to center horizontally
                Padding = new Padding(0, 0, 0, 0),
                BackColor = Operating_Systems.Background,
                AutoScroll = false
            };

            // Main Panel (Acts as the fixed-width content holder)
            Panel contentHolder = new Panel
            {
                Size = new Size(PanelWidth, 750), // Set a fixed height that accommodates all elements
                BackColor = Operating_Systems.Background,
                Margin = new Padding(0)
            };
            currentY += 32;

            // ---------------------------
            // 2. File Path (Input + Browse Button)
            // ---------------------------
            Label labelInputPath = new Label
            {
                Text = "Path",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += labelInputPath.Height + 6;

            Panel pathPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 36),  // Full width -> Best!!
                BackColor = Color.FromArgb(43, 43, 43),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8, 4, 8, 4)
            };

            Button browseButton = new Button
            {
                Text = "Browse",
                Size = new Size(90, 28),
                Location = new Point(PanelWidth - 95, 3), // Aligned right
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
                Text = $@"C:\Users\{Environment.UserName}\Downloads\File.txt"
            };
            // Event is attached below

            pathPanel.Controls.Add(pathTextBox);
            pathPanel.Controls.Add(browseButton);
            currentY += 36 + VerticalSpacing;

            // ---------------------------
            // 3. Content (Input Area)
            // ---------------------------

            Label contentLabel = new Label
            {
                Text = "Content",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += contentLabel.Height + 6;

            const int ContentPanelHeight = 250; // Increased height to take advantage of remaining space
            Panel contentPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, ContentPanelHeight - 30),
                BackColor = Color.FromArgb(43, 43, 43),
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };

            TextBox contentTextBox = new TextBox
            {
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Color.FromArgb(230, 230, 230),
                BackColor = Color.FromArgb(43, 43, 43),
                Multiline = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.None,

                Size = new Size(PanelWidth, ContentPanelHeight - 46), // Fill content panel
                Location = new Point(8, 8),
            };

            contentPanel.Controls.Add(contentTextBox);
            currentY += ContentPanelHeight + 4; // Minimal vertical space after content

            // ---------------------------
            // 4. Status/Options Row (File Type + Char Count)
            // ---------------------------

            // Character Count Label (Moved closer to the content box)
            Label charCountLabel = new Label
            {
                Text = $"Characters: {contentTextBox.Text.Length}",
                Font = new Font("Segoe UI", 8F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Location = new Point(0, currentY - 30)
            };

            currentY += 25 + VerticalSpacing;
            currentY -= 100;

            // ---------------------------
            // 5. Buttons and Message (Combined into a single row using FlowLayoutPanel)
            // ---------------------------

            // Button/Message container to join them horizontally
            FlowLayoutPanel buttonFlow = new FlowLayoutPanel
            {
                Location = new Point(0, currentY + 60),
                Size = new Size(PanelWidth, 42),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Operating_Systems.Background
            };

            // Write Button
            Button writeButton = new Button
            {
                Text = "Save",
                Size = new Size(150, 42),
                BackColor = Operating_Systems.AccentGreen,
                ForeColor = Color.FromArgb(255, 255, 255),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 10F),
                Cursor = Cursors.Hand,
                Margin = new Padding(0)
            };
            writeButton.FlatAppearance.BorderSize = 0;

            // Clear Button
            Button clearButton = new Button
            {
                Text = "Clear",
                Size = new Size(100, 42),
                BackColor = Color.FromArgb(90, 90, 90),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9.5F),
                Cursor = Cursors.Hand,
                Margin = new Padding(10, 0, 0, 0) // Space from write button
            };
            clearButton.FlatAppearance.BorderSize = 0;

            // Message Label (Takes remaining space)
            Label messageLabel = new Label
            {
                AutoSize = true,
                MaximumSize = new Size(PanelWidth - 150 - 100 - 20, 40),
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                Margin = new Padding(20, 10, 0, 0) // Space from clear button, vertical center
            };

            buttonFlow.Controls.Add(writeButton);
            buttonFlow.Controls.Add(clearButton);
            buttonFlow.Controls.Add(messageLabel);
            currentY += buttonFlow.Height + VerticalSpacing;

            // --- Event Wiring (Cleaned up and moved to the end) ---

            // Expression-bodied member for event handling (using local functions for brevity)
            Action<object, EventArgs> UpdateCharCount = (s, e) =>
                charCountLabel.Text = $"Characters: {contentTextBox.Text.Length}";

            Action<object, EventArgs> ClearControls = (s, e) =>
            {
                contentTextBox.Clear();
                messageLabel.Text = string.Empty;
                contentTextBox.Focus();
            };

            // Attach Events
            contentTextBox.TextChanged += UpdateCharCount.Invoke;
            browseButton.Click += (s, e) => BrowseForFile(pathTextBox);
            writeButton.Click += (s, e) => WriteFile(pathTextBox, contentTextBox, messageLabel);
            clearButton.Click += ClearControls.Invoke;

            // --- Add Controls to Holder ---

            contentHolder.Controls.AddRange(new Control[]
            {
            labelInputPath,
            pathPanel,
            contentLabel,
            contentPanel,
            charCountLabel,
            buttonFlow
            });

            // --- Final Assembly ---
            centerFlowPanel.Controls.Add(contentHolder);
            OperatingSystems.AddToMainContainer(centerFlowPanel);
            pathTextBox.Focus();
        }

        private static void BrowseForFile(TextBox pathTextBox)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.Filter = "Text Files (*.txt)|*.txt|Binary Files (*.bin)|*.bin|All Files (*.*)|*.*";
                dialog.Title = "Select File Location";
                dialog.FileName = Path.GetFileName(pathTextBox.Text);

                if (!string.IsNullOrWhiteSpace(pathTextBox.Text))
                {
                    try { dialog.InitialDirectory = Path.GetDirectoryName(pathTextBox.Text); }
                    catch { }
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                    pathTextBox.Text = dialog.FileName;
            }
        }

        private static void ShowMessage(Label label, string message, Color color)
        {
            label.Text = message;
            label.ForeColor = color;
        }
    }
}
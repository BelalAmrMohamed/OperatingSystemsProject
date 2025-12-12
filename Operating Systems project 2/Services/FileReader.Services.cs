using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class FileReader
    {
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

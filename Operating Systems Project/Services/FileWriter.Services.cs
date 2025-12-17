using System;
using System.IO;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class FileWriter
    {
        private static void WriteFile(string path, string content, Label messageLabel, bool append)
        {
            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(content))
            {
                _ = ShowMessage(messageLabel, "⚠ Please fill path and content.", Operating_Systems.ErrorColor);
                return;
            }

            // If directory doesn't exist, create it
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                _ = ShowMessage(messageLabel, "✓ Created the directory.", Operating_Systems.ErrorColor);
            }

            try
            {
                string filetype = Path.GetExtension(path).ToLower();

                if (append) // Append mode
                {
                    if (filetype == ".bin")
                        AppendToBinaryFile(path, content);

                    else AppendToTextFile(path, content);

                }
                else // Overwrite mode: Clear existing content
                {
                    if (filetype == ".bin")
                        WriteToBinaryFile(path, content);

                    else WriteToTextFile(path, content);
                }

                long size = new FileInfo(path).Length;

                _ = ShowMessage(messageLabel,
                    $"✓ Successfully wrote to '{Path.GetFileName(path)}'  ({size} bytes)",
                    Operating_Systems.SuccessColor);
            }
            catch (Exception ex)
            {
                _ = ShowMessage(messageLabel, $"✗ Error: {ex.Message}", Operating_Systems.ErrorColor);                
            }
        }        
    }
}
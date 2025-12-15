using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Operating_Systems_Project
{
    internal partial class FileWriter
    {
        private static void WriteFile(TextBox pathTextBox, TextBox contentTextBox, Label messageLabel)
        {
            string path = pathTextBox.Text.Trim();
            string content = contentTextBox.Text;

            if (string.IsNullOrWhiteSpace(path) || string.IsNullOrEmpty(content))
            {
                ShowMessage(messageLabel, "⚠ Please fill path and content.", Operating_Systems.ErrorColor);
                return;
            }

            // If directory doesn't exist, create it
            string dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                ShowMessage(messageLabel, "✓ Created the directory.", Operating_Systems.ErrorColor);
            }

            try
            {
                string filetype = Path.GetExtension(path).ToLower();

                if (filetype == ".bin")
                    WriteToBinaryFile(path, content);

                else AppendToTextFile(path, content);


                long size = new FileInfo(path).Length; // C: \Users\Belal Amr\Documents\alsalam\OS Writer.txt

                ShowMessage(messageLabel,
                    $"✓ Successfully wrote to '{Path.GetFileName(path)}'  ({size} bytes)",
                    Operating_Systems.SuccessColor);
            }
            catch (Exception ex)
            {
                ShowMessage(messageLabel, $"✗ Error: {ex.Message}", Operating_Systems.ErrorColor);                
            }
        }
    }
}
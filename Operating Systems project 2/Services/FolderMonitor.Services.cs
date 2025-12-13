using System;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class FolderMonitor
    {
        private static void BrowseForFolder(TextBox pathTextBox)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                    pathTextBox.Text = folderDialog.SelectedPath;
            }
        }
        private static void AddEvent(string message)
        {
            if (_reportTextBox == null || _reportTextBox.IsDisposed) return;

            if (_reportTextBox.InvokeRequired)
            {
                _reportTextBox.Invoke(new Action(() => AddEvent(message)));
                return;
            }

            _eventCount++;
            string timestamp = DateTime.Now.ToString("hh:mm tt");


            _reportTextBox.AppendText($"[{timestamp}] {message}\r\n");
            _reportTextBox.SelectionStart = _reportTextBox.Text.Length;
            _reportTextBox.ScrollToCaret();

            Control parentHolder = _reportTextBox.Parent?.Parent;
            if (parentHolder != null)
            {
                var found = parentHolder.Controls.Find("eventCountLabel", true);
                if (found.Length > 0 && found[0] is Label eventLabel)
                    eventLabel.Text = $"Events: {_eventCount}";
            }
        }
    }
}
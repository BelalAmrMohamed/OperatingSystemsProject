using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Operating_Systems_Project
{
    internal partial class General_IO
    {
        /// <summary>
        /// Show the General IO page.
        /// </summary>
        public static void ShowGeneral_IO(Operating_Systems OS)
        {
            const int PanelWidth = 1104;
            const int VerticalSpacing = 16;
            int currentY = 0; // Tracks vertical position            

            // Header + Refresh button area
            Label HeaderLabel = new Label
            {
                Text = "🔁 General IO",
                Font = new Font("Segoe UI Semibold", 13F, FontStyle.Bold),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Operating_Systems.HeaderColor,
                Location = new Point(25, 8)
            };

            // Refresh button (top-right of the content area)
            var refreshButton = new Button
            {
                Text = "Refresh",
                Size = new Size(100, 30),
                Location = new Point(PanelWidth - 110, 6),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Operating_Systems.TextPrimary,
                FlatStyle = FlatStyle.Flat
            };
            refreshButton.FlatAppearance.BorderSize = 0;

            Label SubHeaderLabel = new Label
            {
                Text = "General Input/Output operations. Click Refresh to re-read values.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Italic),
                AutoSize = true,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Operating_Systems.TextSecondary,
                Location = new Point(0, 40)
            };

            // Keep some space below headers
            currentY = 72;

            // --- Reflection Path controls ---
            Label reflectionLabel = new Label
            {
                Text = "Reflection Path (Assembly.Location)",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += reflectionLabel.Height + 6;

            Panel reflectionPanel = new Panel
            {
                Location = new Point(0, currentY),
                Size = new Size(PanelWidth, 36),
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8)
            };
            TextBox reflectionTextBox = new TextBox
            {
                ReadOnly = true,
                Multiline = false,
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = reflectionPanel.BackColor,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                Cursor = Cursors.IBeam
            };
            reflectionPanel.Controls.Add(reflectionTextBox);
            currentY += reflectionPanel.Height + VerticalSpacing;

            // --- Application Path controls ---
            Label appLabel = new Label
            {
                Text = "Application Path (BaseDirectory)",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += appLabel.Height + 6;

            Panel appPanel = new Panel
            {
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8),
                Size = new Size(PanelWidth, 36),
                Location = new Point(0, currentY),
            };
            TextBox appTextBox = new TextBox
            {
                ReadOnly = true,
                Multiline = false,
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = appPanel.BackColor,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };
            appPanel.Controls.Add(appTextBox);
            currentY += appPanel.Height + VerticalSpacing;

            // --- File & Directory info controls (multiline) ---
            Label fileAndDirLabel = new Label
            {
                Text = "File & Directory Info (for executable)",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += fileAndDirLabel.Height + 6;

            Panel fileAndDirPanel = new Panel
            {
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8),
                Size = new Size(PanelWidth, 100),
                Location = new Point(0, currentY),
            };

            TextBox fileAndDirTextBox = new TextBox
            {
                ReadOnly = true,
                Multiline = true,
                AcceptsReturn = true,
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = fileAndDirPanel.BackColor,
                BorderStyle = BorderStyle.None,
                ScrollBars = ScrollBars.None,
                Dock = DockStyle.Fill,
                WordWrap = true
            };

            fileAndDirPanel.Controls.Add(fileAndDirTextBox);
            currentY += fileAndDirPanel.Height + VerticalSpacing;

            // --- Change Attributes controls ---
            Label attributesLabel = new Label
            {
                Text = "Change Attributes (creates Sample.txt in app folder)",
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(0, currentY)
            };
            currentY += attributesLabel.Height + 6;

            Panel attributesPanel = new Panel
            {
                BackColor = Operating_Systems.PanelColor,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(8),
                Size = new Size(PanelWidth, 60),
                Location = new Point(0, currentY),
            };

            TextBox attributesTextBox = new TextBox
            {
                ReadOnly = true,
                Multiline = true,
                Font = new Font("Segoe UI Semibold", 11F),
                ForeColor = Operating_Systems.TextPrimary,
                BackColor = attributesPanel.BackColor,
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };

            attributesPanel.Controls.Add(attributesTextBox);
            currentY += attributesPanel.Height + VerticalSpacing;

            // --- Fill values function used by Refresh button & initial load ---
            Action refreshValues = () =>
            {
                try
                {
                    reflectionTextBox.Text = GetReflectionPath();
                }
                catch { reflectionTextBox.Text = "Error getting reflection path"; }

                try
                {
                    appTextBox.Text = GetApplicationPath();
                }
                catch { appTextBox.Text = "Error getting application path"; }

                try
                {
                    var exePath = GetReflectionPath();
                    fileAndDirTextBox.Text = GetFileAndDirectoryInfo(exePath);
                }
                catch { fileAndDirTextBox.Text = "Error reading file/directory info"; }

                try
                {
                    string Documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string appFolder = Path.Combine(Documents, "Operating Systems app");
                    Directory.CreateDirectory(appFolder); // ⭐ REQUIRED

                    string path = Path.Combine(appFolder, $"Sample.txt");

                    if (File.Exists(path)) // Delete it if the program made it before
                    {
                        File.SetAttributes(path, FileAttributes.Normal);
                        File.Delete(path);
                    }

                    attributesTextBox.Text = $"Path: {path}\r\n" +
                    ChangeAttributes(path);
                }
                catch (Exception ex)
                {
                    attributesTextBox.Text = $"Error changing attributes: {ex.Message}";
                }
            };

            // Wire refresh button
            refreshButton.Click += (s, e) =>
            {
                try 
                {
                    refreshValues();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.ToString());
                }
            };

            // --- Add Controls to Holder ---
            OS.AddToMainContainer(HeaderLabel);
            OS.AddToMainContainer(refreshButton);
            OS.AddToMainContainer(SubHeaderLabel);

            OS.AddToMainContainer(reflectionLabel);
            OS.AddToMainContainer(reflectionPanel);

            OS.AddToMainContainer(appLabel);
            OS.AddToMainContainer(appPanel);

            OS.AddToMainContainer(fileAndDirLabel);
            OS.AddToMainContainer(fileAndDirPanel);

            OS.AddToMainContainer(attributesLabel);
            OS.AddToMainContainer(attributesPanel);

            // Initial load
            refreshValues();
        }
    }
}

using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Operating_Systems_Project
{
    internal partial class WMI
    {
        private static Panel resultsPanel;
        private static Label copyMessageLabel;

        // Message constants
        private static readonly string DefaultCopyMessage = "Double click any box to copy its information";
        private static readonly string SuccessfulCopyingMessage = "✓ Content copied to your clipboard";

        public static void ShowWMI(Operating_Systems OS)
        {
            // --- Layout Configuration ---
            // MainContainer size is approx (1134, 545). 
            // We set width close to this so the Right Anchor distance is small.
            const int InitialWidth = 1100;
            const int LeftMargin = 20; // Increased slightly for better spacing

            // Start lower to avoid the Hamburger Button (Top-Left)
            int currentY = 30;

            // --- 1. Header Section ---
            Label HeaderLabel = new Label
            {
                Text = "💻 WMI Explorer",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Operating_Systems.HeaderColor,
                AutoSize = true,
                Location = new Point(LeftMargin, currentY)
            };
            OS.AddToMainContainer(HeaderLabel);
            currentY += HeaderLabel.Height + 5;

            Label SubHeaderLabel = new Label
            {
                Text = "Windows Management Instrumentation query tool.",
                Font = new Font("Segoe UI", 10F, FontStyle.Regular),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Location = new Point(LeftMargin + 2, currentY)
            };
            OS.AddToMainContainer(SubHeaderLabel);
            currentY += SubHeaderLabel.Height + 20;

            // --- 2. Query Selection ---
            Label queryLabel = new Label
            {
                Text = "Select a System Query",
                Font = new Font("Segoe UI Semibold", 10F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(LeftMargin, currentY)
            };
            OS.AddToMainContainer(queryLabel);
            currentY += queryLabel.Height + 5;

            ComboBox querySelector = new ComboBox
            {
                Location = new Point(LeftMargin, currentY),
                Size = new Size(520, 35),
                BackColor = Operating_Systems.PanelColor,
                ForeColor = Operating_Systems.TextPrimary,
                FlatStyle = FlatStyle.Flat,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                // Anchor prevents it from moving weirdly if form resizes, though Top|Left is default
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            //// Populate Items
            querySelector.Items.AddRange(new object[] {
    // original / common
    "Win32_PerfFormattedData_PerfOS_Memory",
    "Win32_LogicalDisk",
    "Win32_Desktop WHERE Name = '.Default'",
    "Win32_ComputerSystem",
    "Win32_ComputerSystem (Type)",
    "Win32_ComputerSystem (Rename Computer)",
    "Win32_ComputerSystemProduct",
    "Win32_BootConfiguration",
    "Win32_CodecFile",
    "Win32_CodecFile WHERE Group='Video'",
    "Win32_CodecFile WHERE Group='Audio'",
    "Win32_Service",
    "Win32_Service WHERE State='running'",
    "Win32_Service WHERE State='stopped'",
    "Win32_OperatingSystem",
    "Win32_Group",
    "Win32_Group WHERE LocalAccount = 'true'",
    "Win32_UserAccount",
    "Win32_CDROMDrive",
    "Win32_Processor",
    "Win32_Battery", // 
    "Win32_Share",



    // expanded list
    "Win32_BaseBoard",
    "Win32_BIOS",
    "Win32_QuickFixEngineering",                     // installed hotfixes / updates
    "Win32_PnPEntity",
    "Win32_PnPSignedDriver",
    "Win32_USBController",
    "Win32_USBControllerDevice",
    "Win32_NetworkAdapter",
    "Win32_NetworkAdapter WHERE NetEnabled = true",
    "Win32_NetworkAdapterConfiguration WHERE IPEnabled = true",
    "Win32_DiskDrive",
    "Win32_DiskPartition",
    "Win32_Volume",
    "Win32_PhysicalMemory",
    "Win32_PhysicalMemoryArray",
    "Win32_LogicalMemoryConfiguration",
    "Win32_Service WHERE StartMode = 'Auto'",
    "Win32_Service WHERE StartMode = 'Manual'",
    "Win32_SystemDriver",
    "Win32_SystemSlot",
    "Win32_Process",
    "Win32_ScheduledJob",
    "Win32_StartupCommand",
    "Win32_LoggedOnUser",
    "Win32_Printer",
    "Win32_PrinterConfiguration",
    "Win32_PrinterDriver",
    "Win32_Product",                                 // NOTE: can be slow / triggers MSI reconfiguration
    "Win32_Environment",
    "Win32_TimeZone",
    "Win32_OperatingSystem WHERE ProductType = 1",   // desktop OS only
    "Win32_ComputerSystem WHERE PartOfDomain = true",
    "Win32_Service WHERE State='running' AND StartMode='Auto'",

});



            if (querySelector.Items.Count > 0) querySelector.SelectedIndex = 0;
            OS.AddToMainContainer(querySelector);

            Button runQueryButton = new Button
            {
                Text = "Run Query",
                Size = new Size(120, 32),
                Location = new Point(LeftMargin + 410 + 120, currentY - 1),
                BackColor = Operating_Systems.AccentBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI Semibold", 9.5F),
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            runQueryButton.FlatAppearance.BorderSize = 0;
            OS.AddToMainContainer(runQueryButton);

            currentY += querySelector.Height + 20;

            // --- 3. Filter Options (Grouped Logic) ---

            // GROUP 1: Query Scope (All vs Specific)
            Panel scopeGroupPanel = new Panel
            {
                Location = new Point(LeftMargin, currentY),
                Size = new Size(350, 30),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            RadioButton rbtnAllInfo = new RadioButton
            {
                Text = "All Information",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextPrimary,
                Checked = true,
                AutoSize = true,
                Location = new Point(0, 3)
            };

            RadioButton rbtnSpecificInfo = new RadioButton
            {
                Text = "Important Information",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextPrimary,
                AutoSize = true,
                Location = new Point(120, 3)
            };

            scopeGroupPanel.Controls.Add(rbtnAllInfo);
            scopeGroupPanel.Controls.Add(rbtnSpecificInfo);
            OS.AddToMainContainer(scopeGroupPanel);

            // GROUP 2: Empty Value Handling
            Panel filterGroupPanel = new Panel
            {
                Location = new Point(LeftMargin + 360, currentY),
                Size = new Size(400, 30),
                BackColor = Color.Transparent,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };

            RadioButton rbtnHideEmpty = new RadioButton
            {
                Text = "Hide Empty Values",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                Checked = true,
                AutoSize = true,
                Location = new Point(0, 3)
            };

            RadioButton rbtnShowAllValues = new RadioButton
            {
                Text = "Show Empty Values",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Operating_Systems.TextSecondary,
                AutoSize = true,
                Location = new Point(140, 3)
            };

            filterGroupPanel.Controls.Add(rbtnHideEmpty);
            filterGroupPanel.Controls.Add(rbtnShowAllValues);
            OS.AddToMainContainer(filterGroupPanel);

            currentY += 40;

            // --- Event Logic for Groups ---
            rbtnSpecificInfo.CheckedChanged += (s, e) =>
            {
                filterGroupPanel.Visible = !rbtnSpecificInfo.Checked;
            };

            // --- 4. Hints & Messages ---
            copyMessageLabel = new Label
            {
                Text = DefaultCopyMessage,
                AutoSize = true,
                ForeColor = Operating_Systems.AccentBlue,
                Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                Location = new Point(LeftMargin, currentY),
                Visible = false
            };
            OS.AddToMainContainer(copyMessageLabel);
            currentY += 25;

            // --- 5. Results Area ---
            // Calculate remaining height
            // Approx Form Height (592) - Top Margins (~250) - Bottom Padding (50)
            int resultHeight = Math.Max(200, 545 - currentY - 20);

            resultsPanel = new Panel
            {
                Location = new Point(LeftMargin, currentY), // Uses LeftMargin
                // Initial size is critical for Anchors to know the "distance" to the edge
                Size = new Size(InitialWidth, resultHeight),
                BackColor = Operating_Systems.PanelColor,
                AutoScroll = true,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(10),
                // CRITICAL FIX: This makes the panel resize with the form
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };
            OS.AddToMainContainer(resultsPanel);

            // --- 6. Execution Logic ---
            runQueryButton.Click += async (s, e) =>
            {
                runQueryButton.Enabled = false;
                runQueryButton.Text = "Processing...";
                resultsPanel.Controls.Clear();

                if (!copyMessageLabel.Visible) copyMessageLabel.Visible = true;
                copyMessageLabel.Text = DefaultCopyMessage;
                copyMessageLabel.ForeColor = Operating_Systems.TextSecondary;

                string query = querySelector.SelectedItem?.ToString() ?? "";

                try
                {
                    var result = await Task.Run(() =>
                    {
                        if (query == "Win32_ComputerSystem (Rename Computer)")
                            return new[] { RenameComputer() };
                        else if (query == "Win32_ComputerSystem (Type)")
                            return new[] { GetComputerType() };

                        else if (rbtnAllInfo.Checked)
                            return rbtnShowAllValues.Checked ? GetAllQueryInfo(query) : GetQueryInfo(query);

                        else return GetMatchingQuery(query, rbtnShowAllValues.Checked);
                    });

                    ShowQuery_MultiTextBoxes(result);
                }
                catch (Exception ex)
                {
                    Label errorLabel = new Label
                    {
                        Text = "Error executing query:\n" + ex.Message,
                        AutoSize = true,
                        ForeColor = Color.Red,
                        Font = new Font("Segoe UI", 10F),
                        Padding = new Padding(10)
                    };
                    resultsPanel.Controls.Add(errorLabel);
                }
                finally
                {
                    runQueryButton.Enabled = true;
                    runQueryButton.Text = "Run Query";
                }
            };
        }
    }
}
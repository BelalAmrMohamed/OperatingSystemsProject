# Changelog

All notable changes to **Operating Systems Project** will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/)  
and this project follows **Semantic Versioning**.

---

## [4.7.0] 2025-12-24

### Added

- The ability to resize the Form.
- The ability to choose **Important Information** from the WMI tool.
- A message in the `Refresh` button in the `General_IO` section.
- Improved loading performance. Noticable when you add a background image from the Experimental Settings.

## [4.6.0] 2025-12-17

### Added

- Added a **Show Empty Values** option in WMI.
- Added an **Append Content** option in FileWriter.

---

### Changed

- Improved WMI.Logic.cs syntax.
- Improved WMI copying.
- Totaly redid the WMI.Logic.cs

## [4.5.1] 2025-12-15

### Changed

- Removed useless code from WMI, specifically the **ShowQuery_MultiTextBoxes** Method

## [4.5.0] 2025-12-15

### Added

- Clipboard support for WMI resutls.
- Stop ability for the TimerAccuracy class.

---

### Changed

- Improved WMI resutls panel.
- The hidden page now requires 2 clicks instead of 1.

---

### Fixed

- The default path conflict between FileWriter and General IO.

## [4.4.2]

### Changed

- Improved the default file and folder paths

---

### Fixed

- The MainForm.resx Error, previously the app icon was saved as binary data in MainForm.resx. Now the icon is set from the MainForm Constructor

## [4.4.1]

### Fixed

- A FileName Error

## [4.4.0]

### Added

- Added some missing mehtods from FileWriter and FileReader.

---

### Changed

- The loading screen.

---

### Fixed

- The Title bar now turns white when Light mode is enabled.
- The butons' hover effect and click effect in the side panel now matches the theme.
- The conflict between MainForm.cs and MainForm.UI.cs: VS Studio 2022 thought MainForm.UI.cs was an independent form so I merged it into MainForm.cs
- Removed useless Resources

## [4.3.0] 2025-12-12

### Added

- Introduced the General IO section.
- Replaced Splash.cs with the improved Loading.cs
- The missing methods from FileWriter and FileReader
- Improved and cleaned the WMI query logic in WMI.cs after adding them to their own class WMI.Logic.cs

---

### Changed

- Reconstructed the project's Folder structure

---

### Fixed

- Namespace mismatch

## [v4.2.3] - 2025-12-10

### Fixed

- Fixed splash screen flickering under certain conditions.
- Resolved About.config parsing errors for the version field.
- Minor dark-theme UI adjustments.
- Improved loading behavior for the About page.

## [v4.2.2] - 2025-12-01

### Added

- Added automatic version reading from About.config.

### Fixed

- Stability improvements for Folder Monitor.

## [v4.2.1] - 2025-11-27

### Changed

- Updated WMI performance and query handling.

## [v4.2.0] - 2025-11-20

### Fixed

- Initial minor fixes and internal cleanup.

---

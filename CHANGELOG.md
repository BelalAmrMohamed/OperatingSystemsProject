# Changelog

All notable changes to **Operating Systems Project** will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/)  
and this project follows **Semantic Versioning**.

---

## [Pending]

### Additions

- Document the project in Arabic
- Add a main general View instead of defaulting to one of the Section.
- The dark/light mode switch from the Experimental Settings.
- The Background image switch from the Experimental Settings.

---

## [4.4.2]

### Fixed

- The MainForm.resx Error, previously the app icon was saved as binary data in MainForm.resx. Now the icon is set from the MainForm Constructor

---

## [4.4.1]

### Fixed

- A FileName Error

---

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

---

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

---

## [v4.2.3] - 2025-12-10

### Fixed

- Fixed splash screen flickering under certain conditions.
- Resolved About.config parsing errors for the version field.
- Minor dark-theme UI adjustments.
- Improved loading behavior for the About page.

---

## [v4.2.2] - 2025-12-01

### Added

- Added automatic version reading from About.config.

### Fixed

- Stability improvements for Folder Monitor.

---

## [v4.2.1] - 2025-11-27

### Changed

- Updated WMI performance and query handling.

---

## [v4.2.0] - 2025-11-20

### Fixed

- Initial minor fixes and internal cleanup.

---

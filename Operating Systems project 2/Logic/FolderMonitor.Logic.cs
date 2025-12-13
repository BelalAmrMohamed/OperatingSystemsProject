using System.IO;

namespace Operating_Systems_Project
{
    internal partial class FolderMonitor
    {
        private static FileSystemWatcher _watcher;
        private static void StartMonitoring(string path, bool includeSubdirectories)
        {
            StopMonitoring();

            _watcher = new FileSystemWatcher(path);
            _watcher.IncludeSubdirectories = includeSubdirectories;

            _watcher.Changed += OnChanged;
            _watcher.Created += OnCreated;
            _watcher.Deleted += OnDeleted;
            _watcher.Renamed += OnRenamed;

            _watcher.EnableRaisingEvents = true;
        }

        private static void StopMonitoring()
        {
            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
                _watcher = null;
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e) =>
            AddEvent($"Modified: {e.Name}");

        private static void OnDeleted(object sender, FileSystemEventArgs e) =>
            AddEvent($"Deleted: {e.Name}");

        private static void OnRenamed(object sender, RenamedEventArgs e) =>
            AddEvent($"Renamed: {e.OldName} → {e.Name}");

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string type = Directory.Exists(e.FullPath) ? "Folder" : "File";
            AddEvent($"Created: {e.Name} ({type})");
        }
    }
}
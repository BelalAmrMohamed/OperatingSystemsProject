using System;
using System.Reflection;
using System.IO;

namespace Operating_Systems_Project
{
    internal partial class General_IO
    {
        // Get Executing Application's Path with Reflection
        private static string GetReflectionPath()
        {
            return Assembly.GetExecutingAssembly().Location;
        }

        // Get Executing Application's Path
        private static string GetApplicationPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        private static string GetFileAndDirectoryInfo(string path)
        {
            FileInfo file = new FileInfo(path);
            string info = string.Empty;

            info += $"File Name: {file.Name}\r\n"; // properly splits the lines for the WinForms
            info += $"Full Path: {file.FullName}\r\n";

            DirectoryInfo dir = new DirectoryInfo(".");

            info += $"Directory Name: {dir.Name}\r\n";
            info += $"Full Path: {dir.FullName}";

            return info;
        }

        private static string ChangeAttributes(string path)
        {            
            File.WriteAllText(path, "Hello World");

            File.SetAttributes(path, FileAttributes.ReadOnly);
            return $"Attributes Changed: {File.GetAttributes(path)}";
        }
    }
}
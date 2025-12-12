using System.IO;

namespace Operating_Systems_Project
{
    internal partial class FileWriter
    {
        private static void WriteToTextFile(string path, string content)
        {
            // looks for the file, and creates it if it doesn't exist
            using (FileStream fileS = new FileStream(path, FileMode.Append)) 
            using (BufferedStream buffer = new BufferedStream(fileS))
            using (StreamWriter writer = new StreamWriter(buffer))
                writer.WriteLine(content);
        }

        private static void CreateAndWriteToTextFile(string path, string content)
        {
            // Creates a new file
            using (FileStream fileS = new FileStream(path, FileMode.Create)) 
            using (BufferedStream buffer = new BufferedStream(fileS))
            using (StreamWriter writer = new StreamWriter(buffer))
                writer.WriteLine(content);
        }

        private static void WriteAllText(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        private static void WriteToBinaryFile(string path, string content)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
                writer.Write(content);
        }
    }
}

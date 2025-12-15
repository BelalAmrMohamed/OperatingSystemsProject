using System.IO;

namespace Operating_Systems_Project
{
    internal partial class FileWriter
    {
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

        private static void CreateAndWriteAllLinesToTextFile(string path, string[] content) 
        {
            using (StreamWriter Writer = File.CreateText(path))
            {
                // Writer.Write(content);
                for (int i = 0; i < content.Length; i ++)
                    Writer.WriteLine(content[i]);                
            }
        }

        private static void AppendToTextFile(string path, string content)
        {
            // looks for the file, and creates it if it doesn't exist            
            using (StreamWriter writer = File.AppendText(path))
                writer.WriteLine(content);
        }

        private static void CreateAndWriteToBinaryFile(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (BinaryWriter writer = new BinaryWriter(fs))
                writer.Write(content);
        }

        // =====================
        // This Method is extra ( We didn't take it in the section )
        // =====================
        private static void WriteToBinaryFile(string path, string content)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append)))
                writer.Write(content);
        }
    }
}
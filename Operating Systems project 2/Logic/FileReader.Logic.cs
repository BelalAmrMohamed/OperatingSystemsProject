using System.IO;
using System.Text;

namespace Operating_Systems_Project
{
    internal partial class FileReader
    {
        private static string ReadTextFile(string path)
        {
            using (FileStream fileS = new FileStream(path, FileMode.Open, FileAccess.Read))
            using (BufferedStream buffer = new BufferedStream(fileS))
            using (StreamReader reader = new StreamReader(buffer))
                return reader.ReadToEnd();
        }

        private static string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        private static string ReadBinaryFile(string path) // Returns text
        {
            byte[] bytes;

            using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open)))
                bytes = reader.ReadBytes((int)reader.BaseStream.Length);

            return Encoding.UTF8.GetString(bytes);
        }

        private static string ReadBinaryFileAsHexadecimal(string path) // Returns Hexadecimal
        {
            byte[] bytes = File.ReadAllBytes(path);

            StringBuilder hex = new StringBuilder(bytes.Length * 3);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:X2} ", b);

            return hex.ToString().Trim(); // Remove last space
        }
    }
}

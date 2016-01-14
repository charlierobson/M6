using System.IO;
using M6.Classes;

namespace M6
{
    class FileSystemHelper : IFileSystemHelper
    {
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }
    }
}

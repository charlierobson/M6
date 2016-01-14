namespace M6.Classes
{
    public interface IFileSystemHelper
    {
        byte[] ReadAllBytes(string path);
        bool FileExists(string path);
    }
}
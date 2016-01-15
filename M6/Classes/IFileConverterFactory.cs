namespace M6.Classes
{
    public interface IFileConverterFactory
    {
        IFileConverter ParseFile(string path);
    }
}
namespace M6.Classes
{
    public interface IFileConverterFactory
    {
        IFileConverter GetBuilderFor(string path);
    }
}
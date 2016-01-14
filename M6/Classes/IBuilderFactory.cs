namespace M6.Classes
{
    public interface IBuilderFactory
    {
        IFrameDataBuilder GetBuilderFor(string path);
    }
}
namespace M6.Classes
{
    public interface IFrameData
    {
        int Length { get; }
        float[] Left { get; }
        float[] Right { get; }
        IFrameData GetSubset(int start, int count);
    }
}
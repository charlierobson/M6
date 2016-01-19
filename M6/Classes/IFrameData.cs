namespace M6.Classes
{
    public interface IFrameData
    {
        int Length { get; }
        int Resolution { get; }
        float[] Left { get; }
        float[] Right { get; }
        IFrameData GetSubset(int start, int count);
    }
}
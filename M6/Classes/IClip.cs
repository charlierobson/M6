namespace M6.Classes
{
    public interface IClip
    {
        int SampleFrameCount { get; }
        IFrameData GetFrames(int start, int count);
    }
}
namespace M6.Classes
{
    public class Clip : IClip
    {
        private readonly IFrameData _frameData;

        public Clip(IFrameData frameData)
        {
            _frameData = frameData;
        }

        public int SampleFrameCount {
            get { return _frameData.Length; }
        }

        public IFrameData GetFrames(int start, int count)
        {
            return _frameData.GetSubset(start, count);
        }
    }
}
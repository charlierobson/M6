namespace M6.Classes
{
    public class InterleavedSampleData : ISampleData
    {
        public InterleavedSampleData(int channels, float[] sampleData)
        {
            Channels = channels;
            SampleData = sampleData;
        }

        public float[] SampleData { get; private set; }
        public int Channels { get; private set; }
    }

    public interface ISampleData
    {
        float[] SampleData { get; }
        int Channels { get; }
    }
}

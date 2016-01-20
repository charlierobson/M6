using ProtoBuf;

namespace M6.Classes
{
    public interface IFrameData
    {
        int Length { get; }
        int Resolution { get; }
        float[] Left { get; }
        float[] Right { get; }

        IFrameDataSubset GetSubset(int start, int count);

        void BeginChunkyRead(int chunkSize);
        bool ReadChunk(ref IFrameDataSubset subset);
    }
}

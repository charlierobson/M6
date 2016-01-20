using System;

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

    public interface IFrameDataSubset
    {
        int Length { get; }
        ArraySegment<float> Left { get; }
        ArraySegment<float> Right { get; }
    }
}

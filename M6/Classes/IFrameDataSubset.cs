using System;

namespace M6.Classes
{
    public interface IFrameDataSubset
    {
        int Length { get; }
        ArraySegment<float> Left { get; }
        ArraySegment<float> Right { get; }
    }
}
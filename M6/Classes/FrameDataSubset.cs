using System;

namespace M6.Classes
{
    public class FrameDataSubset : IFrameDataSubset
    {
        public FrameDataSubset(float[] left, float[] right, int start, int count)
        {
            Left = new ArraySegment<float>(left, start, count);
            Right = new ArraySegment<float>(right, start, count);
        }

        public int Length
        {
            get { return Left.Count; }
        }

        public ArraySegment<float> Left { get; private set; }
        public ArraySegment<float> Right { get; private set; }
    }
}
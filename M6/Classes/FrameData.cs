using System;

namespace M6.Classes
{
    public class FrameData : IFrameData
    {
        private readonly float[] _leftChannel;
        private readonly float[] _rightChannel;

        private int _chunkReadIndex;
        private int _chunkReadSize;

        public FrameData(float[] left, float[] right, int resolution = 1)
        {
            if (right != null && left.Length != right.Length) throw new ArgumentException();

            _leftChannel = left;
            _rightChannel = right;

            Resolution = resolution;
        }

        public int Length { get { return _leftChannel.Length; } }

        public int Resolution { get; private set; }

        public float[] Left { get { return _leftChannel; } }
        public float[] Right { get { return _rightChannel; } }

        public IFrameDataSubset GetSubset(int start, int count)
        {
            if (start + count > _leftChannel.Length)
            {
                count = _leftChannel.Length - start;
            }

            return new FrameDataSubset(Left, Right, start, count);
        }

        public void BeginChunkyRead(int chunkSize)
        {
            _chunkReadIndex = 0;
            _chunkReadSize = chunkSize;
        }

        public bool ReadChunk(ref IFrameDataSubset subset)
        {
            if (_chunkReadIndex >= _leftChannel.Length) return false;

            var readSize = _chunkReadSize;
            var readIndex = _chunkReadIndex;

            if (readIndex + readSize >= _leftChannel.Length)
            {
                readSize = _leftChannel.Length - readIndex;
            }

            _chunkReadIndex += readSize;

            subset = new FrameDataSubset(Left, Right, readIndex, readSize);
            return true;
        }
    }

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

using System;
using ProtoBuf;

namespace M6.Classes
{
    [ProtoContract]
    public class FrameData : IFrameData
    {
        [ProtoMember(1)]
        public float[] Left { get; private set; }

        [ProtoMember(2)]
        public float[] Right { get; private set; }

        [ProtoMember(3)]
        public int Resolution { get; private set; }

        public int Length { get { return Left.Length; } }

        private int _chunkReadIndex;
        private int _chunkReadSize;

        public FrameData()
        {
            // required for protobuffer deserialisation
        }

        public FrameData(float[] left, float[] right, int resolution = 1)
        {
            if (right != null && left.Length != right.Length) throw new ArgumentException();

            Left = left;
            Right = right;

            Resolution = resolution;
        }

        public IFrameDataSubset GetSubset(int start, int count)
        {
            if (start + count > Left.Length)
            {
                count = Left.Length - start;
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
            if (_chunkReadIndex >= Left.Length) return false;

            var readSize = _chunkReadSize;
            var readIndex = _chunkReadIndex;

            if (readIndex + readSize >= Left.Length)
            {
                readSize = Left.Length - readIndex;
            }

            _chunkReadIndex += readSize;

            subset = new FrameDataSubset(Left, Right, readIndex, readSize);
            return true;
        }
    }
}

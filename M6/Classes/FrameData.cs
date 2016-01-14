using System;

namespace M6.Classes
{
    public class FrameData : IFrameData
    {
        private readonly float[] _leftChannel;
        private readonly float[] _rightChannel;

        public FrameData(float[] left, float[] right)
        {
            if (right != null && left.Length != right.Length) throw new ArgumentException();

            _leftChannel = left;
            _rightChannel = left;
        }

        public int Length { get { return _leftChannel.Length; } }

        public float[] Left { get { return _leftChannel; } }
        public float[] Right { get { return _rightChannel; } }

        public IFrameData GetSubset(int start, int count)
        {
            if (start + count > _leftChannel.Length) return null;

            var leftSubset = new float[count];
            Array.Copy(_leftChannel, start, leftSubset, 0, count);

            var rightSubset = new float[count];
            Array.Copy(_rightChannel, start, rightSubset, 0, count);

            return new FrameData(leftSubset, rightSubset);
        }
    }
}
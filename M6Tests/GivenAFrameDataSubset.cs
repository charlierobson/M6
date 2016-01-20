using System;
using System.Linq;
using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenAFrameDataSubset
    {
        [Fact]
        public void AFrameDataSubsetRepresentsAViewIntoAFrameBuffer()
        {
            var left = new float[100];
            var right = new float[100];
            for (var i = 0; i < 100; ++i)
            {
                left[i] = i;
                right[i] = 99 - i;
            }

            var fds = new FrameDataSubset(left, right, 0, 50);

            Assert.Equal(50, fds.Length);
            Assert.Equal(49, fds.Left.Max());
            Assert.Equal(99, fds.Right.Max());

            fds = new FrameDataSubset(left, right, 50, 50);
            Assert.Equal(50, fds.Length);
            Assert.Equal(99, fds.Left.Max());
            Assert.Equal(49, fds.Right.Max());
        }

        [Fact]
        public void ASubsetCannotExtendPastTheBoundsOfTheParentData()
        {
            var left = new float[100];
            var fds = new FrameDataSubset(left, left, 99, 1);
            Assert.Equal(1, fds.Length);

            Assert.Throws(typeof(ArgumentException), () => fds = new FrameDataSubset(left, left, 99, 2));
        }

        [Fact]
        public void ASubsetCannotHaveANullDataReference()
        {
            float[] x = {};
            Assert.Throws(typeof(ArgumentNullException), () => new FrameDataSubset(null, x, 0, 0));
            Assert.Throws(typeof(ArgumentNullException), () => new FrameDataSubset(x, null, 0, 0));
        }
    }
}

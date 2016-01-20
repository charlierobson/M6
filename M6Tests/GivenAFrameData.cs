using System;
using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenAFrameData
    {
        [Fact]
        public void FrameDataConstructionWillThrowIfMismatchedFrameBuffersAreSupplied()
        {
            // ReSharper disable once UnusedVariable
            Assert.Throws(typeof(ArgumentException), () => { var dummy = new FrameData(new float[4], new float[5]); });
        }

        [Fact]
        public void FrameDataCopyingWillSucceedIfArrayBoundsAreHonoured()
        {
            var sourceData = new FrameData(new float[4], new float[4]);
            Assert.NotNull(sourceData.GetSubset(3, 1));
        }

        [Theory]
        [InlineData(1, 3, 2)]
        [InlineData(0, 4, 2)]
        public void SubsetWillTruncateIfArrayBoundsAreExceededByStartPlusCount(int expected, int start, int count)
        {
            var sourceData = new FrameData(new float[4], new float[4]);
            Assert.Equal(expected, sourceData.GetSubset(start, count).Length);
        }

        [Fact]
        public void GetSubsetWillThrowIfStartIsLargerThanSizeOfSourceData()
        {
            var sourceData = new FrameData(new float[4], new float[4]);
            Assert.Throws(typeof(ArgumentOutOfRangeException), () => { sourceData.GetSubset(5, 1); });
        }
    }
}

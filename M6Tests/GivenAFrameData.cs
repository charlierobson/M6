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

        [Fact]
        public void FrameDataCopyingWillFailIfArrayBoundsAreExceeded()
        {
            var sourceData = new FrameData(new float[4], new float[4]);
            Assert.Null(sourceData.GetSubset(3, 2));
        }
    }
}

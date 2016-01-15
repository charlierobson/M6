using M6.Classes;
using Moq;
using Xunit;

namespace M6Tests
{
    public class GivenAClip
    {
        [Fact]
        public void ALoadedClipExposesItslengthInSampleFrames()
        {
            var mockFrames = new Mock<IFrameData>();
            mockFrames.Setup(f => f.Length).Returns(123);

            var clip = new Clip(mockFrames.Object);

            Assert.Equal(123, clip.SampleFrameCount);
        }

        [Fact]
        public void AClipExposesSampleFrameData()
        {
            var mockSubFrames = new Mock<IFrameData>();
            mockSubFrames.Setup(f => f.Length).Returns(200);

            var mockFrames = new Mock<IFrameData>();
            mockFrames.Setup(f => f.Length).Returns(123);
            mockFrames.Setup(f => f.GetSubset(It.IsAny<int>(), It.IsAny<int>())).Returns(mockSubFrames.Object);

            var clip = new Clip(mockFrames.Object);

            var frameData = clip.GetFrames(100, 200);

            Assert.Equal(200, frameData.Length);
        }
    }
}

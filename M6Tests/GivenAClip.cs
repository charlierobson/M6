using M6.Classes;
using Moq;
using Xunit;

namespace M6Tests
{
    public class GivenAClip
    {
        [Fact]
        public void ClipFactoryReturnsNullWhenClipCannotBeCreatedBecauseFactoryWasntAbleToCreateBuilder()
        {
            var mockBuilderFactory = new Mock<IBuilderFactory>();
            mockBuilderFactory.Setup(b => b.GetBuilderFor(It.IsAny<string>())).Returns((IFrameDataBuilder)null);

            var clip = new ClipFactory(mockBuilderFactory.Object).GetClip("clip1.type");

            Assert.Null(clip);
        }

        [Fact]
        public void ALoadedClipExposesItslengthInSampleFrames()
        {
            var mockFileSystemHelper = new Mock<IFileSystemHelper>();
            mockFileSystemHelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var mockFrames = new Mock<IFrameData>();
            mockFrames.Setup(f => f.Length).Returns(123);

            var mockWavBuilder = new Mock<IFrameDataBuilder>();
            mockWavBuilder.Setup(w => w.Build()).Returns(mockFrames.Object);

            var mockBuilderFactory = new Mock<IBuilderFactory>();
            mockBuilderFactory.Setup(b => b.GetBuilderFor(It.IsAny<string>())).Returns(mockWavBuilder.Object);

            var clip = new ClipFactory(mockBuilderFactory.Object).GetClip("clip1.type");

            Assert.Equal(123, clip.SampleFrameCount);
        }

        [Fact]
        public void AClipExposesSampleFrameData()
        {
            var mockFileSystemHelper = new Mock<IFileSystemHelper>();
            mockFileSystemHelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var mockSubFrames = new Mock<IFrameData>();
            mockSubFrames.Setup(f => f.Length).Returns(200);

            var mockFrames = new Mock<IFrameData>();
            mockFrames.Setup(f => f.Length).Returns(123);
            mockFrames.Setup(f => f.GetSubset(It.IsAny<int>(), It.IsAny<int>())).Returns(mockSubFrames.Object);

            var mockWavBuilder = new Mock<IFrameDataBuilder>();
            mockWavBuilder.Setup(w => w.Build()).Returns(mockFrames.Object);

            var mockBuilderFactory = new Mock<IBuilderFactory>();
            mockBuilderFactory.Setup(b => b.GetBuilderFor(It.IsAny<string>())).Returns(mockWavBuilder.Object);

            var clip = new ClipFactory(mockBuilderFactory.Object).GetClip("clip1.type");

            var frameData = clip.GetFrames(100, 200);

            Assert.Equal(200, frameData.Length);
        }
    }
}

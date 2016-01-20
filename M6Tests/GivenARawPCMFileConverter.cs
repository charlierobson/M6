using M6.Classes;
using Moq;
using Xunit;

namespace M6Tests
{
    public class GivenARawPCMFileConverter
    {
        [Fact]
        public void ARawPCMConversionYieldsAFrameCountOfTheInputPCMSizeInBytesOverFour()
        {
            var mockFileSystemHelper = new Mock<IFileSystemHelper>();
            mockFileSystemHelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);
            mockFileSystemHelper.Setup(f => f.ReadAllBytes(It.IsAny<string>())).Returns(new byte[1234*2*2]);

            var converter = new RawFloatPCMFileConverter("some path", mockFileSystemHelper.Object);

            var frameData = converter.ProcessFile();

            Assert.Equal(1234, frameData.Length);
        }
    }
}

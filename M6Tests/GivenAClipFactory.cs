using M6.Classes;
using Moq;
using Xunit;

namespace M6Tests
{
    public class GivenAClipFactory
    {
        [Fact]
        public void ClipFactoryReturnsNullWhenClipCannotBeCreatedBecauseFactoryWasntAbleToCreateBuilder()
        {
            var mockBuilderFactory = new Mock<IFileConverterFactory>();
            mockBuilderFactory.Setup(b => b.GetBuilderFor(It.IsAny<string>())).Returns((IFileConverter)null);

            var clip = new ClipFactory(mockBuilderFactory.Object).GetClip("clip1.type");

            Assert.Null(clip);
        }

        [Fact]
        public void ClipFactoryReturnsNullIfABuilderFails()
        {
            var mockBuilder = new Mock<IFileConverter>();
            mockBuilder.Setup(b => b.ProcessFile()).Returns((IFrameData)null);

            var mockBuilderFactory = new Mock<IFileConverterFactory>();
            mockBuilderFactory.Setup(b => b.GetBuilderFor(It.IsAny<string>())).Returns(mockBuilder.Object);

            var clip = new ClipFactory(mockBuilderFactory.Object).GetClip("clip1.type");

            Assert.Null(clip);
        }
    }
}

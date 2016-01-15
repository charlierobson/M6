using M6.Classes;
using Moq;
using Xunit;

namespace M6Tests
{
    public class GivenAFileConverterFactory
    {
        [Theory]
        [InlineData(".raw")]
        [InlineData(".mp3")]
        public void ABuilderWillBeReturnedIfAKnownExtensionIsRecognisedAndTheFileExists(string extension)
        {
            var mockFileSystemhelper = new Mock<IFileSystemHelper>();
            mockFileSystemhelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var factory = new FileConverterFactory(mockFileSystemhelper.Object);

            Assert.NotNull(factory.ParseFile("abc" + extension));
        }

        [Theory]
        [InlineData(".mP3")]
        [InlineData(".Mp3")]
        [InlineData(".MP3")]
        [InlineData(".raw")]
        [InlineData(".rAw")]
        [InlineData(".RAW")]
        public void BuilderExtensionComaprisonsAreCaseInsensitive(string path)
        {
            var mockFileSystemhelper = new Mock<IFileSystemHelper>();
            mockFileSystemhelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var factory = new FileConverterFactory(mockFileSystemhelper.Object);

            Assert.NotNull(factory.ParseFile(path));
        }

        [Theory]
        [InlineData(".wav")]
        [InlineData(".flac")]
        public void NoBuilderWillBeReturnedIfTheFileExistsButExtensionDoesNotMatchAKnownType(string extension)
        {
            var mockFileSystemhelper = new Mock<IFileSystemHelper>();
            mockFileSystemhelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(true);

            var factory = new FileConverterFactory(mockFileSystemhelper.Object);
            Assert.Null(factory.ParseFile("abc" + extension));
        }

        [Fact]
        public void NoBuilderWillBeReturnedIfTheFileDoesntExists()
        {
            var mockFileSystemhelper = new Mock<IFileSystemHelper>();
            mockFileSystemhelper.Setup(f => f.FileExists(It.IsAny<string>())).Returns(false);

            var factory = new FileConverterFactory(mockFileSystemhelper.Object);
            Assert.Null(factory.ParseFile("irrelevant"));
        }
    }
}

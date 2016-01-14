using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenABuilderFactory
    {
        [Theory]
        [InlineData(".raw")]
        public void ABuilderWillBeReturnedIfAKnownExtensionIsRecognised(string extension)
        {
            var factory = new BuilderFactory();
            Assert.NotNull(factory.GetBuilderFor("abc" + extension));
        }

        [Fact]
        public void BuilderExtensionComaprisonsAreCaseInsensitive()
        {
            var factory = new BuilderFactory();
            Assert.NotNull(factory.GetBuilderFor("abc.rAw"));
        }

        [Theory]
        [InlineData(".mp3")]
        [InlineData(".wav")]
        [InlineData(".flac")]
        public void NoBuilderWillBeReturnedIfTheExtensionDoesNotMatchAKnownType(string extension)
        {
            var factory = new BuilderFactory();
            Assert.Null(factory.GetBuilderFor("abc" + extension));
        }
    }
}

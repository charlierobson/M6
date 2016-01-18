using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenARange
    {
        [Theory]
        [InlineData(0, 1000)]
        [InlineData(0, 2000)]
        [InlineData(0, 4000)]
        [InlineData(2000, 4000)]
        [InlineData(1500, 2500)]
        [InlineData(3000, 4000)]
        public void TheseRangesIntersectOrAreContainedByTheViewport(int min, int max)
        {
            var viewport = new Range(1000, 3000);
            var testRange = new Range(min, max);
            Assert.True(viewport.ContainsOrIntersectsWithRange(testRange));
        }

        [Theory]
        [InlineData(0, 999)]
        [InlineData(3001, 4000)]
        public void TheseRangesDoNotAppearInTheViewport(int min, int max)
        {
            var viewport = new Range(1000, 3000);
            var testRange = new Range(min, max);
            Assert.False(viewport.ContainsOrIntersectsWithRange(testRange));
        }
    }
}

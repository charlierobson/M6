using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenAWaveSummary
    {
        private readonly WaveSummaryBuilder _waveSummaryBuilder;

        public GivenAWaveSummary()
        {
            _waveSummaryBuilder = new WaveSummaryBuilder();
        }

        [Theory]
        [InlineData(64, 127, 2)]
        [InlineData(64, 128, 2)]
        [InlineData(64, 129, 3)]
        [InlineData(256, 1023, 4)]
        [InlineData(256, 1024, 4)]
        [InlineData(256, 1025, 5)]
        public void AnInputSetOfACertainSizeWillCondenseToAKnownSize(int condensationRatio, int inputSetSize, int expectedOutputSetSize)
        {
            var frameData = new FrameData(new float[inputSetSize], new float[inputSetSize]);
            var summary = _waveSummaryBuilder.MakeSummaryData(frameData, condensationRatio);
            Assert.Equal(expectedOutputSetSize, summary.Length);
        }

        [Fact]
        public void TheSummaryDataWillCondenseTheSampleSetDownToASetOfLocalMaxima()
        {
            var exampleData = new float[1024];
            exampleData[0x012] = 0.8f;
            exampleData[0x013] = -0.7f;
            exampleData[0x134] = 0.75f;
            exampleData[0x135] = -0.5f;
            exampleData[0x256] = 0.29f;
            exampleData[0x257] = -0.3f;
            exampleData[0x378] = 0.99f;
            exampleData[0x379] = -0.98f;

            var frameData = new FrameData(exampleData, exampleData);
            var summary = _waveSummaryBuilder.MakeSummaryData(frameData, 0x100);

            var resultSet = new[]{ 0.8f, 0.75f, 0.3f, 0.99f };

            Assert.Equal(resultSet, summary.Left);
        }
    }
}

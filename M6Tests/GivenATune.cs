using M6.Classes;
using Moq;
using Xunit;

namespace M6Tests
{
    public class GivenATune
    {
        private readonly Mock<IFrameData> _mockFrameData;
        private readonly Mock<IMixProperties> _mockMixProperties;

        public GivenATune()
        {
            _mockFrameData = new Mock<IFrameData>();
            _mockFrameData.Setup(f => f.Frames).Returns(100000);

            _mockMixProperties = new Mock<IMixProperties>();
            _mockMixProperties.Setup(mmp => mmp.PlaybackRateInSamplesPerSecond).Returns(44100);
        }

        [Theory]
        [InlineData(22050, 50000)]
        [InlineData(88200, 200000)]
        [InlineData(44100, 100000)]
        public void ATuneWhichHasARateDifferentToTheMixDefaultWillReportItsEndTickScaledByTheRightAmount(int rate, int endTick)
        {
            var tune = new Tune(_mockMixProperties.Object, _mockFrameData.Object)
            {
                BitRate = rate
            };

            Assert.Equal(endTick, tune.EndTick);
        }

        [Fact]
        public void ATuneWhichPlaysAtDoubleSpeedTakesHalfTheTicksToComplete()
        {
            var tune = new Tune(_mockMixProperties.Object, _mockFrameData.Object)
            {
                BitRate = 22050
            };

            var range = tune.TickRangeToFrameRange(0, 50000);

            Assert.Equal(range.Width, 100000);
        }

        //[Theory]
        //[InlineData(44100, 1000)]
        //[InlineData(22050, 500)]
        //[InlineData(88200, 2000)]
        //public void NumTicksIsRelativeToPlaybackRate(int rate, int lastTick)
        //{
        //    var t = new NuToon(999999, 1000, rate);
        //    Assert.Equal(lastTick, t.LengthInTicks);
        //}

        //[Theory]
        //[InlineData(44100)]
        //[InlineData(22050)]
        //[InlineData(88200)]
        //public void EndTickIsRelativeToPlaybackRateAndStartTick(int rate)
        //{
        //    var t = new NuToon(100000, 1000, rate);
        //    Assert.Equal(t.StartTick + t.LengthInTicks, t.EndTick);
        //}

        //[Fact]
        //public void TicksAreEqulToFramesAtParityRates()
        //{
        //    var t = new NuToon(0, 1000, 44100);
        //    Assert.Equal(t.LengthInTicks, t.LengthInFrames);
        //}

        //[Theory]
        //[InlineData(250, 500)]
        //[InlineData(500, 1000)]
        //public void ATickCanBeMappedToASampleFrame(int tick, int expectedFrame)
        //{
        //    var t = new NuToon(0, 1000, 22050);
        //    Assert.Equal(expectedFrame, t.TickToFrame(tick));
        //}

        //[Theory]
        //[InlineData(1000, 44100, 501000)]
        //[InlineData(1000, 22050, 500500)]
        //public void ASampleFrameCanBeMappedToATick(int frame, int rate, int expectedTick)
        //{
        //    var t = new NuToon(500000, 1000, rate);
        //    Assert.Equal(expectedTick, t.FrameToTick(frame));
        //}
    }
}

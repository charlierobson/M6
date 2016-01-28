using System;
using M6.Classes;
using Moq;
using Xunit;
using Range = M6.Classes.Range;

namespace M6Tests
{
    public class GivenADesktop
    {
        private readonly Mock<IFrameData> _mockFrameData;
        private readonly Mock<IMixProperties> _mockMixProperties;

        public GivenADesktop()
        {
            _mockFrameData = new Mock<IFrameData>();
            _mockFrameData.Setup(f => f.Frames).Returns(200);

            _mockMixProperties = new Mock<IMixProperties>();
            _mockMixProperties.Setup(mmp => mmp.PlaybackRateInSamplesPerSecond).Returns(44100);
        }

        [Fact]
        public void TuneStartsWithinDesktopViewRange()
        {
            // |-----|-----|
            // |     |=====|=====|
            // 500  600   700   800

            var window = new Range(500, 700);
            var tune = new Tune(_mockMixProperties.Object, _mockFrameData.Object) { StartTick = 600, BitRate = 44100 };

            var firstVisibleTuneTick = Math.Max(window.Minimum, tune.StartTick);
            Assert.Equal(600, firstVisibleTuneTick);

            var lastVisibleTuneTick = Math.Min(window.Maximum, tune.EndTick);
            Assert.Equal(700, lastVisibleTuneTick);

            var offsetIntoTune = Math.Max(0, window.Minimum - tune.StartTick);
            Assert.Equal(0, offsetIntoTune);

            var visibleTicks = Math.Min(window.Width, Math.Min(window.Maximum - tune.StartTick, tune.EndTick - window.Minimum));
            Assert.Equal(100, visibleTicks);
        }

        [Fact]
        public void TuneStraddlesDesktopViewRange()
        {
            //        |-----|-----|
            //  |=====|===========|=====|
            // 400   500         700   800

            _mockFrameData.Setup(f => f.Frames).Returns(400);

            var window = new Range(500, 700);
            var tune = new Tune(_mockMixProperties.Object, _mockFrameData.Object) { StartTick = 400, BitRate = 44100 };

            var firstVisibleTuneTick = Math.Max(window.Minimum, tune.StartTick);
            Assert.Equal(500, firstVisibleTuneTick);

            var lastVisibleTuneTick = Math.Min(window.Maximum, tune.EndTick);
            Assert.Equal(700, lastVisibleTuneTick);

            var offsetIntoTune = Math.Max(0, window.Minimum - tune.StartTick);
            Assert.Equal(100, offsetIntoTune);

            var visibleTicks = Math.Min(window.Width, Math.Min(window.Maximum - tune.StartTick, tune.EndTick - window.Minimum));
            Assert.Equal(200, visibleTicks);
        }

        [Fact]
        public void TuneEndWithinDesktopViewRange()
        {
            //         |-----------|
            //   |=====|=====|     |
            //  400   500   600   700
            //
            var window = new Range(500, 700);
            var tune = new Tune(_mockMixProperties.Object, _mockFrameData.Object) { StartTick = 400, BitRate = 44100 };

            var firstVisibleTuneTick = Math.Max(window.Minimum, tune.StartTick);
            Assert.Equal(500, firstVisibleTuneTick);

            var lastVisibleTuneTick = Math.Min(window.Maximum, tune.EndTick);
            Assert.Equal(600, lastVisibleTuneTick);

            var offsetIntoTune = Math.Max(0, window.Minimum - tune.StartTick);
            Assert.Equal(100, offsetIntoTune);

            var visibleTicks = Math.Min(window.Width, Math.Min(window.Maximum - tune.StartTick, tune.EndTick - window.Minimum));
            Assert.Equal(100, visibleTicks);
        }

        [Theory]
        [InlineData(400)]
        [InlineData(800)]
        public void TuneNotInRange(int startTick)
        {
            //               |-----|
            //   |=====|     |     |     |=====|
            //  400   500   600   700   800   900
            //

            _mockFrameData.Setup(f => f.Frames).Returns(100);

            var window = new Range(600, 700);
            var tune = new Tune(_mockMixProperties.Object, _mockFrameData.Object) { StartTick = startTick, BitRate = 44100 };

            var visibleTicks = Math.Min(window.Width, Math.Min(window.Maximum - tune.StartTick, tune.EndTick - window.Minimum));
            Assert.Equal(-100, visibleTicks);
        }


    }
}

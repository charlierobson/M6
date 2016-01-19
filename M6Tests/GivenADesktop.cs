using System;
using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenADesktop
    {
        public GivenADesktop()
        {
        }

        [Fact]
        public void TuneStartsWithinDescktopViewRange()
        {
            // |--------ddd-----------|
            // |     |================|======|

            var pixels = 100;
            var ticksPerPixel = 500;

            var sampleWindow = new Range(0, pixels * ticksPerPixel);

            var tune = new Tune(400000) { StartTick = 25000};

            var samplesVisible = sampleWindow.Maximum - tune.StartTick;
            Assert.Equal(25000, samplesVisible);

            var startPixel = tune.StartTick/ticksPerPixel;
            Assert.Equal(50, startPixel);

            // showing more on screen
            ticksPerPixel = 1000;
            sampleWindow.Maximum = pixels * ticksPerPixel;

            Assert.Equal(100000, sampleWindow.Maximum);

            samplesVisible = sampleWindow.Maximum - tune.StartTick;
            Assert.Equal(75000, samplesVisible);

            startPixel = tune.StartTick / ticksPerPixel;
            Assert.Equal(25, startPixel);

            // negative first visible = offset onto desktop
            var firstVisibleTuneTick = sampleWindow.Minimum - tune.StartTick;
            Assert.Equal(-25000, firstVisibleTuneTick);

            var startPixelFromFirstVisible = -firstVisibleTuneTick / ticksPerPixel;
            Assert.Equal(startPixel, startPixelFromFirstVisible);
        }

        [Theory]
        [InlineData(400000)]
        public void TuneStraddlesDesktopViewRange(int tuneStartTick)
        {
            //       |----------------------|
            // |=====|======================|=====|

            var sampleWindow = new Range(500, 700);
            var tune = new Tune(700) { StartTick = 200 };

            var firstVisibleTuneTick = Math.Max(0, sampleWindow.Minimum - tuneStartTick);
            Assert.Equal(300, firstVisibleTuneTick);

            var lastVisibleTuneTick = firstVisibleTuneTick + sampleWindow.Width;
            Assert.Equal(500);
        }
    }
}

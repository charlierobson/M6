using System.Drawing;
using M6.Classes;
using Xunit;

namespace M6Tests
{
    public class GivenADelta
    {
        [Fact]
        public void ADeltaHasZeroForItsInitialValues()
        {
            var d = new Delta();

            Assert.Equal(0, d.DX);
            Assert.Equal(0, d.DY);

            d.Update(new Point(0,0));

            Assert.Equal(0, d.DX);
            Assert.Equal(0, d.DY);
        }

        [Fact]
        public void DeltaUpdatesChangeDxAndDy()
        {
            var d = new Delta();
            d.Update(new Point(23, 45));

            Assert.Equal(23, d.DX);
            Assert.Equal(45, d.DY);
        }

        [Fact]
        public void ResetWillRePositionTheDeltaOrigin()
        {
            var d = new Delta();
            d.Update(new Point(23, 45));

            d.Reset(new Point(24,64));

            Assert.Equal(0, d.DX);
            Assert.Equal(0, d.DY);

            d.Update(new Point(36, 98));

            Assert.Equal(12, d.DX);
            Assert.Equal(34, d.DY);
        }

        [Fact]
        public void NegativeValuesAreValidToo()
        {
            var d = new Delta(new Point(23, 45));

            d.Update(new Point(22, 44));

            Assert.Equal(-1, d.DX);
            Assert.Equal(-1, d.DY);
        }
    }
}

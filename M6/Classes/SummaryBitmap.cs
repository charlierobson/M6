using System.Drawing;
using System.Drawing.Imaging;

namespace M6.Classes
{
    public class SummaryBitmap : ISummaryBitmap
    {
        private Bitmap _bitmap;

        public int Resolution { get; private set; }

        public Bitmap Bitmap
        {
            get { return _bitmap; }
        }

        public SummaryBitmap(IFrameData summaryData)
        {
            Resolution = summaryData.Resolution;

            _bitmap = new Bitmap(summaryData.Length, 250, PixelFormat.Format24bppRgb);

            var graphics = Graphics.FromImage(_bitmap);
            graphics.Clear(Color.CadetBlue);
            graphics.DrawRectangle(Pens.Black, 0, 0, _bitmap.Width - 1, _bitmap.Height - 1);

            var x = 0;
            foreach (var m in summaryData.Left)
            {
                var h = 240 * m;
                var d = (_bitmap.Height - h) / 2;
                graphics.DrawLine(Pens.Black, x, d, x, _bitmap.Height - d);
                ++x;
            }
        }
    }

    public interface ISummaryBitmap
    {
        int Resolution { get; }
        Bitmap Bitmap { get; }
    }
}

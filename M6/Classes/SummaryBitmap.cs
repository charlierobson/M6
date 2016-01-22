using System.Drawing;
using System.Drawing.Imaging;

namespace M6.Classes
{
    public class SummaryBitmap : ISummaryBitmap
    {
        private readonly Bitmap _bitmap;

        public int Resolution { get; private set; }

        public Bitmap Bitmap
        {
            get { return _bitmap; }
        }

        public SummaryBitmap(IFrameData summaryData, int summaryHeight, IFrameData onsetData)
        {
            Resolution = summaryData.Resolution;

            _bitmap = new Bitmap(summaryData.Length, summaryHeight, PixelFormat.Format24bppRgb);

            summaryHeight -= 6;

            var graphics = Graphics.FromImage(_bitmap);
            graphics.Clear(Color.CadetBlue);
            graphics.DrawRectangle(Pens.Black, 0, 0, _bitmap.Width - 1, _bitmap.Height - 1);

            var x = 0;
            foreach (var m in summaryData.Left)
            {
                var h = summaryHeight * m;
                var d = (_bitmap.Height - h) / 2;
                graphics.DrawLine(Pens.Black, x, d, x, _bitmap.Height - d);
                ++x;
            }

            if (onsetData != null)
            {
                x = 0;
                foreach (var m in onsetData.Left)
                {
                    if (m > 0)
                    {
                        graphics.DrawRectangle(Pens.Chartreuse, x, 0, x+1, _bitmap.Height);
                    }
                    ++x;
                }
            }
        }
    }

    public interface ISummaryBitmap
    {
        int Resolution { get; }
        Bitmap Bitmap { get; }
    }
}

using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace M6
{
    public partial class M6Form : Form
    {
        private bool _down;
        private Point _position;
        private Point _delta;
        //private Bitmap _bitmap;

        public M6Form()
        {
            InitializeComponent();
        }

        private void M6Form_Load(object sender, System.EventArgs e)
        {
/*
            var converter = new FileConverterFactory(new FileSystemHelper()).ParseFile(@"C:\Users\Administrator\99s.mp3");
            if (converter == null) return;

            var waveData = converter.ProcessFile();
            if (waveData == null) return;

            var summary = new WaveSummary();
            var summaryData = summary.MakeSummaryData(waveData, 1024);

            _bitmap = new Bitmap(summaryData.Length, 250, PixelFormat.Format24bppRgb);
            var graphics = Graphics.FromImage(_bitmap);
            graphics.Clear(Color.CadetBlue);

            var x = 0;
            foreach (var m in summaryData.Left)
            {
                var h = 240*m;
                var d = (_bitmap.Height - h) / 2;
                graphics.DrawLine(Pens.Black, x, d, x, _bitmap.Height - d);
                ++x;
            }

            _bitmap.Save(@"C:\Users\Administrator\99s.png");
 */
        }

        private void M6Form_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Goldenrod);

            var clientWidth = ClientRectangle.Width;
            var spacing = 500 / 10;
            var numTicks = clientWidth / spacing;

            for (var i = 0; i < numTicks; ++i)
            {
                e.Graphics.DrawLine(Pens.Black, i * spacing, 0, i * spacing, 20);
            }

            var txt = string.Format("W: {0}", clientWidth);
            e.Graphics.DrawString(txt, DefaultFont, new SolidBrush(Color.Black), 10, 100);

            txt = string.Format("P: {0},{1}", _position.X, _position.Y);
            e.Graphics.DrawString(txt, DefaultFont, new SolidBrush(Color.Black), 10, 120);

            txt = string.Format("D: {0},{1}", _delta.X, _delta.Y);
            e.Graphics.DrawString(txt, DefaultFont, new SolidBrush(Color.Black), 10, 140);
        }

        private void M6Form_ResizeEnd(object sender, System.EventArgs e)
        {
            Invalidate();
        }

        private void M6Form_MouseDown(object sender, MouseEventArgs e)
        {
            _down = true;
            _position = e.Location;
            _delta = new Point(0,0);
            Invalidate();
        }

        private void M6Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_down) return;

            _delta.X = e.Location.X - _position.X;
            _delta.Y = e.Location.Y - _position.Y;
            Invalidate();
        }

        private void M6Form_MouseUp(object sender, MouseEventArgs e)
        {
            _down = false;
        }
    }
}

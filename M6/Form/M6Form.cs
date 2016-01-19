using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using M6.Classes;

namespace M6
{
    public partial class M6Form : Form
    {
        private bool _down;
        private Bitmap _bitmap;

        private int _logy;

        private Bitmap _bbBitmap;

        private Delta _delta;

        private readonly List<Tune> _tunes;

        private int _ticksPerPixel;
        private Range _desktopRange;
        private Tune _selectedTune;

        public M6Form()
        {
            _tunes = new List<Tune>();

            InitializeComponent();
        }

        private void M6Form_Load(object sender, System.EventArgs e)
        {
            _delta = new Delta();

            var converter = new FileConverterFactory(new FileSystemHelper()).ParseFile(@"C:\Users\Administrator\99s.mp3");
            if (converter == null) return;

            var waveData = converter.ProcessFile();
            if (waveData == null) return;

            _ticksPerPixel = 1024;

            var summary = new WaveSummary();
            var summaryData = summary.MakeSummaryData(waveData, _ticksPerPixel);

            _bitmap = new Bitmap(summaryData.Length, 250, PixelFormat.Format24bppRgb);
            var graphics = Graphics.FromImage(_bitmap);
            graphics.Clear(Color.CadetBlue);
            graphics.DrawRectangle(Pens.Black, 0,0, _bitmap.Width-1,_bitmap.Height-1);

            var x = 0;
            foreach (var m in summaryData.Left)
            {
                var h = 240 * m;
                var d = (_bitmap.Height - h)/2;
                graphics.DrawLine(Pens.Black, x, d, x, _bitmap.Height - d);
                ++x;
            }

            _bbBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            _tunes.Add(new Tune(waveData.Length));
            _tunes[0].StartTick = 40000;
            _tunes[0].Track = 0;

            _tunes.Add(new Tune(waveData.Length));
            _tunes[1].StartTick = 1000000;
            _tunes[1].Track = 1;

            _desktopRange = new Range(0, ClientRectangle.Width * _ticksPerPixel);
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // stop the flicker
        }

        private void M6Form_Paint(object sender, PaintEventArgs e)
        {
            var backbuffer = Graphics.FromImage(_bbBitmap);

            backbuffer.Clear(Color.GhostWhite);

            backbuffer.DrawLine(Pens.Black, 0,0,0,20);
            backbuffer.DrawLine(Pens.Black, ClientRectangle.Width - 1, 0, ClientRectangle.Width - 1, 20);

            backbuffer.DrawString(_desktopRange.Minimum.ToString(), DefaultFont, new SolidBrush(Color.Black), 5, 10);

            var rangeMax = _desktopRange.Maximum.ToString();
            var extent = TextRenderer.MeasureText(backbuffer, rangeMax, DefaultFont);
            backbuffer.DrawString(rangeMax, DefaultFont, new SolidBrush(Color.Black), ClientRectangle.Width - 1 - extent.Width - 5, 10);

            Logout();

            foreach (var tune in _tunes)
            {
                var tuneRange = tune.Range;
                if (!_desktopRange.ContainsOrIntersectsWithRange(tuneRange)) continue;

                var firstVisibleTuneTick = Math.Max(_desktopRange.Minimum, tune.StartTick);
                var firstVisibleTunePixel = (firstVisibleTuneTick - _desktopRange.Minimum) /_ticksPerPixel;

                var tickOffsetIntoTune = Math.Max(0, _desktopRange.Minimum - tune.StartTick);
                var visibleTicks = Math.Min(_desktopRange.Width, Math.Min(_desktopRange.Maximum - tune.StartTick, tune.EndTick - _desktopRange.Minimum));

                var dstRect = new Rectangle(firstVisibleTunePixel, 100 + tune.Track * 260, visibleTicks / _ticksPerPixel, 250);

                const int summaryResolution = 1024;
                var srcRect = new Rectangle(tickOffsetIntoTune / summaryResolution, 0, visibleTicks / summaryResolution, 250);

                backbuffer.DrawImage(_bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
                backbuffer.DrawRectangle(Pens.Chartreuse, dstRect);
            }

            Logout(backbuffer, "tpp {0}", _ticksPerPixel);
            e.Graphics.DrawImage(_bbBitmap, 0, 0);
        }

        private void Logout(Graphics g = null, string format = null, params object[] parameters)
        {
            if (g == null || format == null)
            {
                _logy = 30;
                return;
            }

            var text = string.Format(format, parameters);
            g.DrawString(text, DefaultFont, new SolidBrush(Color.Black), 10, _logy);
            _logy += 15;
        }

        private void M6Form_ResizeEnd(object sender, EventArgs e)
        {
            _bbBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            _desktopRange.Maximum = _desktopRange.Minimum + ClientRectangle.Width * _ticksPerPixel;
            Invalidate();
        }

        private void M6Form_MouseDown(object sender, MouseEventArgs e)
        {
            _down = true;
            _delta.Reset(e.Location);

            var tick = _desktopRange.Minimum + e.Location.X * _ticksPerPixel;

            _selectedTune = _tunes.FirstOrDefault(tune => tune.Range.ContainsValue(tick) && new Range(100 + tune.Track*260, 100 + (tune.Track+1)*260).ContainsValue(e.Location.Y));

            Invalidate();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            _ticksPerPixel += e.Delta;
            _desktopRange.Maximum = ClientRectangle.Width * _ticksPerPixel;

            Invalidate();
        }

        private void M6Form_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_down) return;

            _delta.Update(e.Location);
            var dist = _delta.DX * _ticksPerPixel;

            if (_selectedTune != null)
            {
                _selectedTune.StartTick += dist;
            }
            else
            {
                if (_desktopRange.Minimum < dist)
                {
                    dist = _desktopRange.Minimum;
                }

                _desktopRange.Minimum -= dist;
                _desktopRange.Maximum -= dist;
            }

            Invalidate();
        }

        private void M6Form_MouseUp(object sender, MouseEventArgs e)
        {
            _down = false;
        }
    }
}

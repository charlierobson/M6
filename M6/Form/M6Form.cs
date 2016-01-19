using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M6.Classes;
using Newtonsoft.Json;

namespace M6
{
    public partial class M6Form : Form
    {
        private bool _down;

        private int _logy;

        private Bitmap _bbBitmap;

        private Delta _delta;

        private readonly List<Tune> _tunes;

        private int _ticksPerPixel;
        private Range _desktopRange;
        private Tune _selectedTune;
        private SummaryBitmap _summaryBitmap;

        public M6Form()
        {
            _tunes = new List<Tune>();

            InitializeComponent();
        }

        private void M6Form_Load(object sender, System.EventArgs e)
        {
            //string[] tunes = { "paris red - good friend.mp3", "jinny - keep warm.mp3" };
            //File.WriteAllText(".\\tunes.json", JsonConvert.SerializeObject(tunes));

            var files = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(".\\tunes.json"));
            foreach (var file in files)
            {
                var converter = new FileConverterFactory(new FileSystemHelper()).ParseFile(file);
                if (converter == null) continue;

                var waveData = converter.ProcessFile();
                if (waveData == null) continue;

                var tune = new Tune(waveData);
                tune.BuildSummaries();
                tune.StartTick = 0;
                tune.Track = _tunes.Count;

                _tunes.Add(tune);
            }

            _delta = new Delta();

            _ticksPerPixel = 1024;

            var summaryData = _tunes[0].Summary(_ticksPerPixel);
            _summaryBitmap = new SummaryBitmap(summaryData);

            _bbBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

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

                Logout(backbuffer, "{0}", tune.Track);

                var firstVisibleTuneTick = Math.Max(_desktopRange.Minimum, tune.StartTick);
                var firstVisibleTunePixel = (firstVisibleTuneTick - _desktopRange.Minimum) /_ticksPerPixel;

                var tickOffsetIntoTune = Math.Max(0, _desktopRange.Minimum - tune.StartTick);
                var visibleTicks = Math.Min(_desktopRange.Width, Math.Min(_desktopRange.Maximum - tune.StartTick, tune.EndTick - _desktopRange.Minimum));

                var summaryData = tune.Summary(_ticksPerPixel);
                if (summaryData.Resolution != _summaryBitmap.Resolution)
                {
                    _summaryBitmap = new SummaryBitmap(summaryData);
                }

                var dstRect = new Rectangle(firstVisibleTunePixel, 100 + tune.Track * 260, visibleTicks / _ticksPerPixel, 250);
                var srcRect = new Rectangle(tickOffsetIntoTune / _summaryBitmap.Resolution, 0, visibleTicks / _summaryBitmap.Resolution, 250);

                backbuffer.DrawImage(_summaryBitmap.Bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
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

            var fraction = (double)e.Location.X / ClientRectangle.Width;
            var tickAtCursor = _desktopRange.Minimum + (e.Location.X * _ticksPerPixel);

            if (_ticksPerPixel + e.Delta <= 128) return;

            _ticksPerPixel += e.Delta;

            var newTickWidth = ClientRectangle.Width*_ticksPerPixel;

            _desktopRange.Minimum = (int) (tickAtCursor - (newTickWidth*fraction));
            _desktopRange.Maximum = _desktopRange.Minimum + newTickWidth;

            if (_desktopRange.Minimum < 0)
            {
                _desktopRange.Minimum = 0;
                _desktopRange.Maximum = newTickWidth;
            }
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

        private void buttonFit_Click(object sender, EventArgs e)
        {
            _desktopRange.Maximum = _tunes.Select(tune => tune.EndTick).Concat(new[] { 0 }).Max();
            _desktopRange.Minimum = _tunes.Select(tune => tune.StartTick).Concat(new[] {Int32.MaxValue}).Min();
            _ticksPerPixel = _desktopRange.Width/ClientRectangle.Width;
            Invalidate();
        }
    }
}

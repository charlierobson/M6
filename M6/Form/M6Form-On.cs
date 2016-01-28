using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M6.Classes;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

// ReSharper disable LocalizableElement

namespace M6.Form
{
    public partial class M6Form
    {
        private const int WfHeight = 125;

        private bool _leftButtonDown;

        private int _logY;
        private int _ticksPerPixel;
        private int _playCursor;

        private WaveOut _waveOut;
        private Bitmap _bbBitmap;
        private Delta _delta;
        private Project _project;
        private Range _desktopRange;
        private Tune _selectedTune;

        private void On_Load()
        {
            //var p = new TuneSeed[]
            //{
            //    new TuneSeed(){BitRate = 44100, Filename = "1", StartTick = 123},
            //    new TuneSeed(){BitRate = 44100, Filename = "2", StartTick = 123}
            //};
            //var x = JsonConvert.SerializeObject(p);

            try
            {
                _project = Project.OpenProject(Path.Combine(Directory.GetCurrentDirectory(), "TestProject"));

                _delta = new Delta();

                _ticksPerPixel = 1024;

                _bbBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

                _desktopRange = new Range(0, ClientRectangle.Width * _ticksPerPixel);
            }
            catch
            {
                //
            }
        }

        private void Logout(Graphics g = null, string format = null, params object[] parameters)
        {
            if (g == null || format == null)
            {
                _logY = 30;
                return;
            }

            var text = string.Format(format, parameters);
            g.DrawString(text, DefaultFont, new SolidBrush(Color.Black), 10, _logY);
            _logY += 15;
        }

        private void On_Paint(PaintEventArgs e)
        {
            var backbuffer = Graphics.FromImage(_bbBitmap);

            backbuffer.Clear(Color.GhostWhite);

            backbuffer.DrawLine(Pens.Black, 0, 0, 0, 20);
            backbuffer.DrawLine(Pens.Black, ClientRectangle.Width - 1, 0, ClientRectangle.Width - 1, 20);

            backbuffer.DrawString(_desktopRange.Minimum.ToString(), DefaultFont, new SolidBrush(Color.Black), 5, 10);

            var rangeMax = _desktopRange.Maximum.ToString();
            var extent = TextRenderer.MeasureText(backbuffer, rangeMax, DefaultFont);
            backbuffer.DrawString(rangeMax, DefaultFont, new SolidBrush(Color.Black), ClientRectangle.Width - 1 - extent.Width - 5, 10);

            Logout();

            foreach (var tune in _project.Tunes)
            {
                var tuneRange = tune.TickRange;
                if (!_desktopRange.ContainsOrIntersectsWithRange(tuneRange)) continue;

                var firstVisibleTuneTick = Math.Max(_desktopRange.Minimum, tune.StartTick);
                var visibleTicks = Math.Min(_desktopRange.Width, Math.Min(_desktopRange.Maximum - tune.StartTick, tune.EndTick - _desktopRange.Minimum));

                var firstVisibleTunePixel = (firstVisibleTuneTick - _desktopRange.Minimum) / _ticksPerPixel;

                var frameRange = tune.TickRangeToFrameRange(firstVisibleTuneTick, visibleTicks);

                var summaryData = tune.Summary(_ticksPerPixel);
                if (tune.SummaryBitmap == null || summaryData.Resolution != tune.SummaryBitmap.Resolution)
                {
                    tune.SummaryBitmap = new SummaryBitmap(summaryData, WfHeight, tune.Onsets(_ticksPerPixel));
                }

                var dstRect = new Rectangle(firstVisibleTunePixel, 100 + tune.Track * (WfHeight + 10), visibleTicks / _ticksPerPixel, WfHeight);
                var srcRect = new Rectangle(frameRange.Minimum / tune.SummaryBitmap.Resolution, 0, frameRange.Width / tune.SummaryBitmap.Resolution, WfHeight);

                backbuffer.DrawImage(tune.SummaryBitmap.Bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
                if (_selectedTune == tune)
                {
                    backbuffer.DrawRectangle(Pens.Aqua, dstRect.X, dstRect.Y, dstRect.Width - 1, dstRect.Height - 1);
                    backbuffer.DrawRectangle(Pens.Aqua, dstRect.X, dstRect.Y+1, dstRect.Width - 1, dstRect.Height - 3);
                }
            }

            var x = TickToPixel(_playCursor);
            backbuffer.DrawLine(Pens.Red, x, 0, x, ClientRectangle.Height);

            e.Graphics.DrawImage(_bbBitmap, 0, 0);
        }

        private int TickToPixel(int tick)
        {
            return (tick - _desktopRange.Minimum)/_ticksPerPixel;
        }


        private void On_ResizeEnd()
        {
            _bbBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            _desktopRange.Maximum = _desktopRange.Minimum + ClientRectangle.Width * _ticksPerPixel;
            Invalidate();
        }

        private void On_MouseDown(MouseEventArgs e)
        {
            _leftButtonDown = true;
            _delta.Reset(e.Location);

            var clickedTick = _desktopRange.Minimum + e.Location.X * _ticksPerPixel;

            _selectedTune = _project.Tunes.FirstOrDefault(tune => tune.TickRange.ContainsValue(clickedTick) && new Range(100 + tune.Track * (WfHeight + 10), 100 + (tune.Track + 1) * (WfHeight + 10)).ContainsValue(e.Location.Y));

            if (_selectedTune == null)
            {
                _playCursor = clickedTick;
            }

            Invalidate();
        }

        private void On_MouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if ((Control.ModifierKeys & Keys.Control) == Keys.Control)
            {
                if (_selectedTune != null)
                {
                    _selectedTune.BitRate += e.Delta / 8;
                }
            }
            else
            {
                var fraction = (double) e.Location.X/ClientRectangle.Width;
                var tickAtCursor = _desktopRange.Minimum + (e.Location.X*_ticksPerPixel);

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
            }
            Invalidate();
        }

        private void On_MouseMove(MouseEventArgs e)
        {
            if (!_leftButtonDown) return;

            _delta.Update(e.Location);
            var dist = _delta.DX * _ticksPerPixel;

            if (_selectedTune != null)
            {
                _selectedTune.StartTick += dist;
                if (_selectedTune.StartTick < 0)
                    _selectedTune.StartTick = 0;
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

        private void On_MouseUp()
        {
            _leftButtonDown = false;
        }

        private void On_buttonFit_Click()
        {
            _desktopRange.Maximum = _project.Tunes.Select(tune => tune.EndTick).Concat(new[] { 0 }).Max();
            _desktopRange.Minimum = _project.Tunes.Select(tune => tune.StartTick).Concat(new[] { Int32.MaxValue }).Min();
            _ticksPerPixel = _desktopRange.Width / ClientRectangle.Width;
            Invalidate();
        }

        private void On_buttonPlay_Click(object sender)
        {
            var buttonSender = (Button)sender;

            if (_waveOut == null)
            {
                buttonSender.Text = "Stop";

                _waveOut = new WaveOut();

                var providers = new[]
                {
                    new M6SampleProvider(_project.Tunes[0], _playCursor),
                    new M6SampleProvider(_project.Tunes[1], _playCursor)
                };

                _waveOut.Init(new MixingSampleProvider(providers));
                _waveOut.Play();
            }
            else
            {
                buttonSender.Text = "Play";

                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
        }

        private void On_TestButton()
        {
        }
    }
}

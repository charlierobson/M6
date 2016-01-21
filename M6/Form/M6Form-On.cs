using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using M6.Classes;
using NAudio.Wave;
using ProtoBuf;

namespace M6.Form
{
    public partial class M6Form
    {
        private bool _leftButtonDown;

        private int _logy;

        private Bitmap _bbBitmap;

        private Delta _delta;

        private List<Tune> _tunes;

        private int _ticksPerPixel;
        private Range _desktopRange;
        private Tune _selectedTune;

        private void On_Load()
        {
            _tunes = new List<Tune>();

            //var p = new Project { TuneFilenames = new[] { "abc", "def" } };
            //string x = JsonConvert.SerializeObject(p);

            var fileConverterFactory = new FileConverterFactory(new FileSystemHelper());

            var project = Project.OpenProject(@".\TestProject\");

            foreach (var fileName in project.TuneFilenames)
            {
                var metaDataPath = Path.Combine(project.WorkingFolder, "MetaData");
                Directory.CreateDirectory(metaDataPath);

                var tunePath = Path.Combine(project.WorkingFolder, fileName);

                var rawTunePath = Path.ChangeExtension(Path.Combine(metaDataPath, fileName), "m6raw");
                var summaryPath = Path.ChangeExtension(Path.Combine(metaDataPath, fileName), "summary");

                IFileConverter converter;
                if ((converter = fileConverterFactory.ParseFile(rawTunePath)) == null)
                {
                    converter = fileConverterFactory.ParseFile(tunePath);
                }
                if (converter == null) continue;

                var waveData = converter.ProcessFile();
                if (waveData == null) continue;

                if (!File.Exists(rawTunePath))
                {
                    try
                    {
                        using (var rawFile = File.Create(rawTunePath))
                        {
                            Serializer.Serialize(rawFile, waveData);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                var tune = new Tune(waveData);

                SummaryCollection summaryData = null;
                try
                {
                    using (var summaryFile = File.OpenRead(summaryPath))
                    {
                        summaryData = Serializer.Deserialize<SummaryCollection>(summaryFile);
                    }

                    tune.SummaryCollection = summaryData;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                if (summaryData == null)
                {
                    tune.BuildSummaries();
                    try
                    {
                        using (var summaryFile = File.Create(summaryPath))
                        {
                            Serializer.Serialize(summaryFile, tune.SummaryCollection);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                }

                tune.StartTick = 0;
                tune.Track = _tunes.Count;

                _tunes.Add(tune);
            }

            _delta = new Delta();

            _ticksPerPixel = 1024;

            _bbBitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);

            _desktopRange = new Range(0, ClientRectangle.Width * _ticksPerPixel);

            var w = new WaveOut();
            w.Init(new M6SampleProvider(_tunes[1]));
            w.Play();
            System.Threading.Thread.Sleep(2000);
            w.Stop();
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

            foreach (var tune in _tunes)
            {
                var tuneRange = tune.TickRange;
                if (!_desktopRange.ContainsOrIntersectsWithRange(tuneRange)) continue;

                Logout(backbuffer, "{0}", tune.Track);

                var firstVisibleTuneTick = Math.Max(_desktopRange.Minimum, tune.StartTick);
                var firstVisibleTunePixel = (firstVisibleTuneTick - _desktopRange.Minimum) / _ticksPerPixel;

                var tickOffsetIntoTune = Math.Max(0, _desktopRange.Minimum - tune.StartTick);
                var visibleTicks = Math.Min(_desktopRange.Width, Math.Min(_desktopRange.Maximum - tune.StartTick, tune.EndTick - _desktopRange.Minimum));

                var summaryData = tune.Summary(_ticksPerPixel);
                if (tune.SummaryBitmap == null || summaryData.Resolution != tune.SummaryBitmap.Resolution)
                {
                    tune.SummaryBitmap = new SummaryBitmap(summaryData, tune.Onsets(_ticksPerPixel));
                }

                var dstRect = new Rectangle(firstVisibleTunePixel, 100 + tune.Track * 260, visibleTicks / _ticksPerPixel, 250);
                var srcRect = new Rectangle(tickOffsetIntoTune / tune.SummaryBitmap.Resolution, 0, visibleTicks / tune.SummaryBitmap.Resolution, 250);

                backbuffer.DrawImage(tune.SummaryBitmap.Bitmap, dstRect, srcRect, GraphicsUnit.Pixel);
            }

            Logout(backbuffer, "tpp {0}", _ticksPerPixel);
            e.Graphics.DrawImage(_bbBitmap, 0, 0);
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

            var tick = _desktopRange.Minimum + e.Location.X * _ticksPerPixel;

            _selectedTune = _tunes.FirstOrDefault(tune => tune.TickRange.ContainsValue(tick) && new Range(100 + tune.Track * 260, 100 + (tune.Track + 1) * 260).ContainsValue(e.Location.Y));

            Invalidate();
        }

        private void On_MouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            var fraction = (double)e.Location.X / ClientRectangle.Width;
            var tickAtCursor = _desktopRange.Minimum + (e.Location.X * _ticksPerPixel);

            if (_ticksPerPixel + e.Delta <= 128) return;

            _ticksPerPixel += e.Delta;

            var newTickWidth = ClientRectangle.Width * _ticksPerPixel;

            _desktopRange.Minimum = (int)(tickAtCursor - (newTickWidth * fraction));
            _desktopRange.Maximum = _desktopRange.Minimum + newTickWidth;

            if (_desktopRange.Minimum < 0)
            {
                _desktopRange.Minimum = 0;
                _desktopRange.Maximum = newTickWidth;
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
            _desktopRange.Maximum = _tunes.Select(tune => tune.EndTick).Concat(new[] { 0 }).Max();
            _desktopRange.Minimum = _tunes.Select(tune => tune.StartTick).Concat(new[] { Int32.MaxValue }).Min();
            _ticksPerPixel = _desktopRange.Width / ClientRectangle.Width;
            Invalidate();
        }
    }
}

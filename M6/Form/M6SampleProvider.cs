using System;
using M6.Classes;
using NAudio.Wave;

namespace M6.Form
{
    public class M6SampleProvider : WaveProvider32
    {
        private readonly object _lockObject = new object();

        private readonly ITune _tune;
        private readonly Action<int> _update;

        private IFrameDataSubset _subset;

        public M6SampleProvider(ITune tune, int playCursorTick, Action<int> update)
        {
            _tune = tune;
            _update = update;

            _tune.FrameData.BeginChunkyRead(playCursorTick - _tune.StartTick);
            _tune.FrameData.ReadChunk(ref _subset);

        }

        public override int Read(float[] buffer, int offset, int count)
        {
            lock (_lockObject)
            {
                if (!_tune.FrameData.ReadChunk(ref _subset, count)) return 0;

                var i = 0;
                foreach (var sample in _subset.Left)
                {
                    buffer[i] = sample;
                    ++i;
                }

                _update(_subset.Left.Offset);

                return _subset.Left.Count;
            }
        }
    }
}
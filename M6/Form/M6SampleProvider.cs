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
        private readonly IFrameDataSubset _subset;

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
                var i = 0;
                IFrameDataSubset subset = null;
                _tune.FrameData.ReadChunk(ref subset, count);
                foreach (var sample in subset.Left)
                {
                    buffer[i] = sample;
                    ++i;
                }

                _update(subset.Left.Offset);

                return count;
            }
        }
    }
}
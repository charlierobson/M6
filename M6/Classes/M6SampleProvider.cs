using System;
using NAudio.Wave;

namespace M6.Classes
{
    public class M6SampleProvider : WaveProvider32
    {
        private readonly object _lockObject = new object();

        private readonly ITune _tune;
        private readonly Resampler _resampler;

        private int _playCursorTick;

        public M6SampleProvider(ITune tune, int playCursorTick)
        {
	        _tune = tune;

            var sourceOffset = 0;
	        if (playCursorTick > _tune.TickRange.Minimum)
	        {
	            sourceOffset = playCursorTick - _tune.TickRange.Minimum;
	        }

            _resampler = new Resampler(tune.FrameData, sourceOffset, tune.BitRate / 44100.0);

            _playCursorTick = playCursorTick;
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            lock (_lockObject)
            {
                if (_tune.TickRange.Minimum <= _playCursorTick)
                {
                    return _resampler.ReadSamples(buffer, offset, count);
                }

                var ticksTillPlayStart = Math.Min(count, _tune.TickRange.Minimum - _playCursorTick);
                _playCursorTick += ticksTillPlayStart;

                return ticksTillPlayStart;
            }
        }
    }
}

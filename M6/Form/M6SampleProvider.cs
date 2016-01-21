using M6.Classes;
using NAudio.Wave;

namespace M6.Form
{
    public class M6SampleProvider : ISampleProvider
    {
        private readonly object _lockObject = new object();

        private readonly ITune _tune;

        public M6SampleProvider(ITune tune)
        {
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);

            _tune = tune;
        }

        public int Read(float[] buffer, int offset, int count)
        {
            lock (_lockObject)
            {
                var i = 0;
                var subset = _tune.Subset(offset, count);
                foreach (var sample in subset.Left)
                {
                    buffer[i] = sample;
                    ++i;
                }

                return count;
            }
        }

        public WaveFormat WaveFormat { get; private set; }
    }
}
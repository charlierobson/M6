using System;
using LibSampleRate;
using M6.Classes;
using NAudio.Wave;

namespace M6.Form
{
	public class Resampler
	{
		private readonly IFrameData _sampleSource;
		private readonly SampleRateConverter _resamplingEngine;

		private int _inputSampleOffset;

		public Resampler(IFrameData sampleSource, int inputSampleOffset)
		{
			_sampleSource = sampleSource;
			_inputSampleOffset = inputSampleOffset;

			_resamplingEngine = new SampleRateConverter(ConverterType.SRC_SINC_BEST_QUALITY, 1);
			_resamplingEngine.SetRatio(0.93);
		}

		public bool ReadSamples(float[] destSampleArray, out int writtenSampleCount)
		{
			int inputSamplesUsed, samplesGenerated;

			var requiredSamples = destSampleArray.Length / 4; // sizeof(float)
			var inputRemainingBytes = _sampleSource.Length - _inputSampleOffset;

			_resamplingEngine.Process(_sampleSource.Left, _inputSampleOffset, inputRemainingBytes, 
				destSampleArray, 0, requiredSamples,
				false,
				out inputSamplesUsed, out samplesGenerated);

			_inputSampleOffset += inputSamplesUsed;
			writtenSampleCount = samplesGenerated;

			return true;
		}
	}

    public class M6SampleProvider : WaveProvider32
    {
        private readonly object _lockObject = new object();

	    private readonly Action<int> _update;

	    private readonly Resampler _resampler;

		private float[] _resampledData;

	    public M6SampleProvider(ITune tune, int playCursorTick, Action<int> update)
        {
		    _update = update;

			_resampledData = null;
			_resampler = new Resampler(tune.FrameData, playCursorTick - tune.StartTick);
        }

        public override int Read(float[] buffer, int offset, int count)
        {
            lock (_lockObject)
            {
	            int bufferedSampleCount;
				return !_resampler.ReadSamples(buffer, out bufferedSampleCount) ? 0 : bufferedSampleCount;
            }
        }
    }

}
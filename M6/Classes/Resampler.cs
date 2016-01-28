using System;
using LibSampleRate;

namespace M6.Classes
{
    public class Resampler
    {
        private readonly IFrameData _sampleSource;
        private readonly SampleRateConverter _resamplingEngine;

        private int _inputSampleOffset;

        public Resampler(IFrameData sampleSource, int inputSampleOffset, double ratio)
        {
            _sampleSource = sampleSource;
            _inputSampleOffset = inputSampleOffset;

            _resamplingEngine = new SampleRateConverter(ConverterType.SRC_SINC_BEST_QUALITY, 1);
            _resamplingEngine.SetRatio(ratio);
        }

        public int ReadSamples(float[] destSampleArray, int offset, int count)
        {
            int inputSamplesUsed, samplesGenerated;

            var inputRemainingBytes = _sampleSource.Frames - _inputSampleOffset;

            _resamplingEngine.Process(_sampleSource.Left, _inputSampleOffset, inputRemainingBytes,
                destSampleArray, offset, count,
                false, // todo
                out inputSamplesUsed, out samplesGenerated);

            _inputSampleOffset += inputSamplesUsed;
            return samplesGenerated;
        }
    }
}

using M6.Classes;

namespace M6.Processing.OnsetDetection
{
    public class AudioAnalysis
    {
        private OnsetDetection _onsetDetection;

        public float[] DetectOnsets(IFrameData waveData, int samplesPerSecond, int sampleWindowSize, float sensitivity = 1.5f)
        {
            _onsetDetection = new OnsetDetection(sampleWindowSize, samplesPerSecond);

            waveData.BeginChunkyRead(sampleWindowSize);

            IFrameDataSubset subset = null;
            while(waveData.ReadChunk(ref subset))
            {
                var mono = ConvertStereoToMono(subset);
                _onsetDetection.AddFlux(mono);
            }

            var onsets = _onsetDetection.FindOnsets(sensitivity);
            _onsetDetection.NormalizeOnsets(onsets, 0);

            return onsets;
        }


        private float[] ConvertStereoToMono(IFrameDataSubset subset)
        {
            var outputIndex = 0;
            var output = new float[subset.Length];
            var offset = subset.Left.Offset;
            for (var i = 0; i < subset.Length; i += 2)
            {
                output[outputIndex] = (subset.Left.Array[i + offset] + subset.Right.Array[i + offset]) / 2;
                outputIndex++;
            }

            return output;
        }
    }    
}

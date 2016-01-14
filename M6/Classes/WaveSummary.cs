using System;
using System.Collections.Generic;
using System.Linq;

namespace M6.Classes
{
    public class WaveSummary : IWaveSummary
    {
        public IFrameData MakeSummaryData(IFrameData input, int ratio)
        {
            var max = new List<float>();

            var sourceLength = input.Length;
            var resampledFrameCount = sourceLength / ratio;

            for (var i = 0; i < resampledFrameCount; ++i)
            {
                var subset = input.GetSubset(i * ratio, ratio);

                var maxmag = Math.Max(subset.Left.Max(), subset.Right.Max());
                var minmag = Math.Min(subset.Left.Min(), subset.Right.Min());
                max.Add(Math.Max(maxmag, Math.Abs(minmag)));
            }

            var blockLen = sourceLength - resampledFrameCount * ratio;
            if (blockLen != 0)
            {
                var subset = input.GetSubset(resampledFrameCount * ratio, blockLen);

                var maxmag = Math.Max(subset.Left.Max(), subset.Right.Max());
                var minmag = Math.Min(subset.Left.Min(), subset.Right.Min());
                max.Add(Math.Max(maxmag, Math.Abs(minmag)));
            }

            return new FrameData(max.ToArray(), null);
        }
    }
}

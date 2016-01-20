using System;
using System.Collections.Generic;
using System.Linq;

namespace M6.Classes
{
    public class WaveSummaryBuilder : IWaveSummary
    {
        public IFrameData MakeSummaryData(IFrameData input, int ratio)
        {
            var max = new List<float>();

            input.BeginChunkyRead(ratio);

            IFrameDataSubset subset = null;
            while(input.ReadChunk(ref subset))
            {
                var maxmag = Math.Max(subset.Left.Max(), subset.Right.Max());
                var minmag = Math.Min(subset.Left.Min(), subset.Right.Min());
                max.Add(Math.Max(maxmag, Math.Abs(minmag)));
            }

            return new FrameData(max.ToArray(), null, ratio);
        }
    }
}

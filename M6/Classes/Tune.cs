namespace M6.Classes
{
    public class Tune : ITune
    {
        private readonly IFrameData _frameData;
        private readonly IFrameData[] _summaries;
        public int StartTick { get; set; }

        public int EndTick
        {
            get { return StartTick + Ticks; }
        }

        public int Ticks
        {
            get { return _frameData.Length; }
        }

        public int Track { get; set; }
        public double Rate { get; set; }

        public Tune(IFrameData frameData)
        {
            _frameData = frameData;
            _summaries = new IFrameData[3];
        }

        public void BuildSummaries()
        {
            var summary = new WaveSummaryBuilder();

            _summaries[2] = summary.MakeSummaryData(_frameData, 16384);
            _summaries[1] = summary.MakeSummaryData(_frameData, 1024);
            _summaries[0] = summary.MakeSummaryData(_frameData, 128);
        }

        public IFrameData Summary(int displayScale)
        {
            if (displayScale > 2000) return _summaries[0];
            else if (displayScale >500) return _summaries[1];
            else return _summaries[2];
        }

        public Range Range
        {
            get { return new Range(StartTick, EndTick); }
        }

        public float Data(int i)
        {
            return _frameData.Left[i];
        }
    }

    public interface ITune
    {
        int StartTick { get; set; }
        int EndTick { get; }
        int Ticks { get; }
        int Track { get; }
        double Rate { get; set; }
        void BuildSummaries();
        IFrameData Summary(int displayScale);
        Range Range { get; }
    }
}

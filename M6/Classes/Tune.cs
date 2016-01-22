using M6.Processing.OnsetDetection;

namespace M6.Classes
{
    public class Tune : ITune
    {
        private readonly IFrameData _frameData;

        public SummaryCollection SummaryCollection;

        private IFrameData _onsets;

        public int StartTick { get; set; }

        public int EndTick
        {
            get { return StartTick + Ticks; }
        }

        public int Ticks
        {
            get { return (int)(BitRate / 44100 * _frameData.Length); }
        }

        public int Track { get; set; }
        public double BitRate { get; set; }

        public Tune(IFrameData frameData)
        {
            _frameData = frameData;
        }

        public void BuildSummaries()
        {
            var builder = new WaveSummaryBuilder();

            SummaryCollection = new SummaryCollection();

            SummaryCollection.Summary[2] = (FrameData)builder.MakeSummaryData(_frameData, 4096);
            SummaryCollection.Summary[1] = (FrameData)builder.MakeSummaryData(_frameData, 1024);
            SummaryCollection.Summary[0] = (FrameData)builder.MakeSummaryData(_frameData, 256);
        }

        public void BuildOnsets()
        {
            var audioAnalysis = new AudioAnalysis();
            var onsetData = audioAnalysis.DetectOnsets(_frameData, 44100, 1024);
            _onsets = new FrameData(onsetData, null, 1024);
        } 

        public IFrameData Summary(int displayScale)
        {
            if (displayScale >= 4096) return SummaryCollection.Summary[2];
            else if (displayScale >= 1024) return SummaryCollection.Summary[1];
            else return SummaryCollection.Summary[0];
        }

        public SummaryBitmap SummaryBitmap { get; set; }

        public IFrameData Onsets(int displayScale)
        {
            return _onsets;
        }

        public Range TickRange
        {
            get { return new Range(StartTick, EndTick); }
        }

        public IFrameData FrameData
        {
            get { return _frameData; }
        }

        public IFrameDataSubset Subset(int startTick, int count)
        {
            return _frameData.GetSubset(startTick, count);
        }
    }
}

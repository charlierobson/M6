using M6.Processing.OnsetDetection;

namespace M6.Classes
{
    public class Tune : ITune
    {
        private readonly IMixProperties _mixProperties;
        private readonly IFrameData _frameData;

        public SummaryCollection SummaryCollection;

        private IFrameData _onsets;

        public double BitRate { get; set; }

        public int LengthInFrames
        {
            get { return _frameData.Frames; }
        }

        public int LengthInTicks
        {
            get { return (int)(LengthInFrames * (BitRate / _mixProperties.PlaybackRateInSamplesPerSecond)); }
        }

        public int StartTick { get; set; }

        public int EndTick
        {
            get { return StartTick + LengthInTicks; }
        }

        public int TickToFrame(int tick)
        {
            tick -= StartTick;
            return (int)(tick / (BitRate / _mixProperties.PlaybackRateInSamplesPerSecond));
        }

        public int FrameToTick(int frame)
        {
            return (int)(StartTick + (frame * (BitRate / _mixProperties.PlaybackRateInSamplesPerSecond)));
        }

        public int Track { get; set; }

        public Tune(IMixProperties mixProperties, IFrameData frameData)
        {
            _mixProperties = mixProperties;
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
            var onsetData = audioAnalysis.DetectOnsets(_frameData, _mixProperties.PlaybackRateInSamplesPerSecond, 1024);
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

        public Range TickRangeToFrameRange(int startTick, int tickCount)
        {
            return new Range(TickToFrame(startTick), TickToFrame(startTick + tickCount));
        }

        public IFrameData FrameData
        {
            get { return _frameData; }
        }

        public IFrameDataSubset Subset(int startFrame, int frameCount)
        {
            return _frameData.GetSubset(startFrame, frameCount);
        }
    }
}

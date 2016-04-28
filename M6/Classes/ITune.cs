namespace M6.Classes
{
    public interface ITune
    {
        Range TickRange { get; }

        double BitRate { get; set; }

        void BuildSummaries();
        IFrameData Summary(int displayScale);

        void BuildOnsets();
        IFrameData Onsets(int displayScale);

        IFrameData FrameData { get; }
        IFrameDataSubset Subset(int startFrame, int frameCount);

        Range TickRangeToFrameRange(int startTick, int tickCount);

        int Track { get; }
    }
}
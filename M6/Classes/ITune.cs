namespace M6.Classes
{
    public interface ITune
    {
        int LengthInTicks { get; }
        int LengthInFrames { get; }

        int StartTick { get; set; }
        int EndTick { get; }

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
namespace M6.Classes
{
    public interface ITune
    {
        int StartTick { get; set; }
        int EndTick { get; }
        int Ticks { get; }
        int Track { get; }

        double Rate { get; set; }

        void BuildSummaries();
        IFrameData Summary(int displayScale);

        void BuildOnsets();
        IFrameData Onsets(int displayScale);

        Range TickRange { get; }

        IFrameDataSubset Subset(int startTick, int count);
    }
}
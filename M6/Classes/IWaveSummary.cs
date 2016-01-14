namespace M6.Classes
{
    public interface IWaveSummary
    {
        IFrameData MakeSummaryData(IFrameData input, int ratio);
    }
}
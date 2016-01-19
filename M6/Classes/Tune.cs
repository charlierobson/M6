namespace M6.Classes
{
    public class Tune : ITune
    {
        private readonly IFrameData _frameData;
        public int StartTick { get; set; }

        public int EndTick
        {
            get { return StartTick + Ticks; }
        }

        public int Ticks { get; private set; }

        public int Track { get; set; }

        public Tune(int ticks)
        {
            Ticks = ticks;
        }

        public Tune(IFrameData d)
        {
            Ticks = d.Length;
            _frameData = d;
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
        Range Range { get; }
    }
}

namespace M6.Classes
{
    public class Tune : ITune
    {
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

        public Range Range
        {
            get { return new Range(StartTick, EndTick); }
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

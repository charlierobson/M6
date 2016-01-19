namespace M6.Classes
{
    public class Range
    {
        public Range(int min, int max)
        {
            Minimum = min;
            Maximum = max;
        }

        public int Minimum { get; set; }
        public int Maximum { get; set; }

        public int Width { get { return Maximum - Minimum; } }

        public bool IsValid() { return Minimum < Maximum; }

        public bool ContainsValue(int value)
        {
            return value >= Minimum && value < Maximum;
        }

        public bool IsInsideRange(Range range)
        {
            return IsValid() && range.IsValid() && range.ContainsValue(Minimum) && range.ContainsValue(Maximum);
        }

        public bool ContainsOrIntersectsWithRange(Range testRange)
        {
            return ContainsValue(testRange.Minimum) || ContainsValue(testRange.Maximum) || IsInsideRange(testRange);
        }
    }
}
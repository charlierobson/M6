namespace M6.Classes
{
    public class Position : IMovable
    {
        public Position(int x)
        {
            X = x;
        }

        // IMovable

        public void Translate(int xDelta)
        {
            X += xDelta;
        }

        public int X { get; set; }
    }
}
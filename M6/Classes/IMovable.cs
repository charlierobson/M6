namespace M6.Classes
{
    public interface IMovable
    {
        void Translate(int xDelta);
        int X { get; set; }
    }
}
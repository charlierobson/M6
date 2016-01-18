using System.Drawing;

namespace M6.Classes
{
    public interface IDelta
    {
        void Reset(Point position);
        void Update(Point newPosition);
        int DX { get; }
        int DY { get; }
    }
}
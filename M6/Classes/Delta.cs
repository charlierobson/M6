using System.Drawing;

namespace M6.Classes
{
    public class Delta : IDelta
    {
        private Size _magnitude;
        private Point _lastPosition;

        public Delta()
        {
        }

        public Delta(Point p)
        {
            Reset(p);
        }

        public void Reset(Point position)
        {
            _lastPosition = position;
            _magnitude.Width = 0;
            _magnitude.Height = 0;
        }

        public void Update(Point newPosition)
        {
            _magnitude.Width = newPosition.X - _lastPosition.X;
            _magnitude.Height = newPosition.Y - _lastPosition.Y;
            _lastPosition = newPosition;
        }

        public int DX { get { return _magnitude.Width; } }
        public int DY { get { return _magnitude.Height; } }
    }
}
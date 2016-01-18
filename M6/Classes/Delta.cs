using System.Drawing;

namespace M6.Classes
{
    public class Delta : IDelta
    {
        private Point _lastPosition = new Point();
        private Size _magnitude;

        public void Reset(Point position)
        {
            _lastPosition = position;
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
namespace PoC.Messaging.Models
{
    internal struct Point
    {
        public Point()
        {
            this.X = 0;
            this.Y = 0;
        }

        public Point(int x, int y)
        {
            X = x; 
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}

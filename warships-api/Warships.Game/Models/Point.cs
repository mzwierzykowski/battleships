namespace Warships.Game.Models
{
    public class Point
    {
        public Point(int x, int y)
        {
            X = x; 
            Y = y;
        }
        public string Id { get { return $"{X}.{Y}"; } }
        public int X { get; private set; }
        public int Y { get; private set; }
        public PointState State { get; set; } = PointState.Unknown;
    }
}

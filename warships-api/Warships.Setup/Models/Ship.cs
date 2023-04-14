namespace Warships.Setup.Models
{
    public class Ship
    {
        public Ship(List<Point> points, string type)
        {
            Points = points;
            Type = type;
        }
        public List<Point> Points { get; private set; }
        public string Type { get; private set; }
    }
}
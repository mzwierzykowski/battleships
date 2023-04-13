using Warships.Configuration;

namespace Warships.Game.Models
{
    public class Board
    {
        public List<Point> Points { get; set; } = new();
        public BoardDimension Dimension { get; set; } = new();
    }
}

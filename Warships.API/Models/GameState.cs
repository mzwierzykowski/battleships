using System.Diagnostics.CodeAnalysis;
using Warships.Game.Models;

namespace Warships.API.Models
{
    [ExcludeFromCodeCoverage]
    public class GameState
    {
        public List<Point> Board { get; set; } = new List<Point>();
        public List<ShipStats> ShipStats { get; set; } = new List<ShipStats>();

        public bool Isfinished { get; set; }
        public int TotalHits { get; set; }
        public int TotalMiss { get; set; }
        public int ShotsFired { get; set; }
    }
}

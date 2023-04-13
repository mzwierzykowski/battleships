using System.Diagnostics.CodeAnalysis;
using Warships.Game.Models;

namespace Warships.API.Models
{
    [ExcludeFromCodeCoverage]
    public class Stats
    {
        public List<ShipStats> ShipStats { get; set; } = new List<ShipStats>();
        public int TotalHits { get; set; }
        public int TotalMiss { get; set; }
        public int ShotsFired { get; set; }
    }
}

namespace Warships.Game.Models
{
    public class GameState
    {
        public Board Board { get; set; } = new() { Points = new List<Point>(), Dimension = new() };
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public List<ShipStats> ShipStats { get { return RecalculateStats(); } } 

        private List<ShipStats> RecalculateStats()
        {
            var stats = new List<ShipStats>();
            var groups = Ships.GroupBy(x => x.Type).ToList();
            foreach (var group in groups) 
            {
                stats.Add(new ShipStats()
                {
                    Type = group.Key,
                    TotalCount = group.Count(),
                    LeftInGameCount = group.Where(x => !x.IsSunk).Count(),
                });
            }
            return stats;
        }

        public bool Isfinished 
        { 
            get { return Ships.Where(s => s.IsSunk).ToList().Count == Ships.Count; } 
        }
        public int TotalHits
        {
            get { return Board.Points.Where(t => t.State == PointState.Hit).ToList().Count; }
        }
        public int TotalMiss
        {
            get { return Board.Points.Where(t => t.State == PointState.Miss).ToList().Count; }
        }
        public int ShotsFired 
        { 
            get { return Board.Points.Where(t => t.State != PointState.Unknown).ToList().Count; } 
        }
    }
}

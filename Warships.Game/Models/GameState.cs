using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Warships.Game.Models
{
    public class GameState
    {
        public List<Point> Board { get; set; } = new List<Point>();
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public List<ShipStats> Stats { get { return RecalculateStats(); } } 

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

        public bool Finished 
        { 
            get { return Ships.Where(s => s.IsSunk).ToList().Count == Ships.Count; } 
        }
        public int TotalHits
        {
            get { return Board.Where(t => t.State == PointState.Hit).ToList().Count; }
        }
        public int TotalMiss
        {
            get { return Board.Where(t => t.State == PointState.Miss).ToList().Count; }
        }
        public int ShotsFired 
        { 
            get { return Board.Where(t => t.State != PointState.Unknown).ToList().Count; } 
        }
    }
}

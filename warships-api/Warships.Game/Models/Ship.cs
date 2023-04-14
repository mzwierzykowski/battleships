using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warships.Game.Models
{
    public class Ship
    {
        public List<Point> Points { get; set; } = new List<Point>();
        public string Type { get; set; } = string.Empty;
        public bool IsSunk { get { return Points.Where(x => x.State == PointState.Hit).ToList().Count == Points.Count; } }
    }
}

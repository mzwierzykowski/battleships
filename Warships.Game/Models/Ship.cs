using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warships.Game.Models
{
    internal class Ship
    {
        public List<Tile> Tiles { get; set; } = new List<Tile>();
        public string Type { get; set; } = string.Empty;
        public bool IsSunk { get { return Tiles.Where(x => x.State == TileState.Hit).ToList().Count == Tiles.Count; } 
        }
    }
}

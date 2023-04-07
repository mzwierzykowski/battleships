using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warships.Game.Models
{
    internal class ShipStats
    {
        public string Type { get; set; } = string.Empty;
        public int TotalCount { get; set; }
        public int LeftInGameCount { get; set; }
    }
}

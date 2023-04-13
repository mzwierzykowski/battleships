using System.Diagnostics.CodeAnalysis;
using Warships.Game.Models;

namespace Warships.API.Models
{
    [ExcludeFromCodeCoverage]
    public class GameState
    {
        public Board Board { get; set; } = new Board() { Points = new(), Dimension = new() };
        public Stats Stats { get; set; } = new Stats();
        public bool Isfinished { get; set; }

    }
}

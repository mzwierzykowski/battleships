namespace Warships.Game.Models
{
    public class Tile
    {
        public string Id { get { return $"{X}.{Y}"; } }
        public int X { get; set; }
        public int Y { get; set; }
        public TileState State { get; set; } = TileState.Unknown;
    }
}

using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IBoardGenerator
    {
        public List<Point> GenerateBoard();
    }
}

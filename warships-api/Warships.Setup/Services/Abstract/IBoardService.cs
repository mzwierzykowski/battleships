using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IBoardService
    {
        public void ResetBoardState();
        public Point GetRandomAvaiablePoint();
        public void RemoveOccupiedArea(Ship ship);
        public Point? DefineNextPoint(Point basePoint, BuildDirection direction);
    }
}

using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IBoardService
    {
        public Point GetRandomAvaiablePoint();
        public void RemoveOccupiedArea(Ship ship);
        public Point? DefineNextPoint(Point basePoint, BuildDirection direction);
    }
}

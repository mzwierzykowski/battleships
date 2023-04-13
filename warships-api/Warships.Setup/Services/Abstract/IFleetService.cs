using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IFleetService
    {
        public List<Ship> BuildFleet();
    }
}

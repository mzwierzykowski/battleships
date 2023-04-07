using Warships.Setup.Models;

namespace Warships.Setup.Services.Abstract
{
    public interface IShipyard
    {
        public Ship BuildShip(string type, int size);
    }
}

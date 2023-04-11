using Warships.Game.Models;

namespace Warships.Game.Services.Abstract
{
    public interface IGameService
    {
        public GameState StartGame();
        public GameState ShootsFired(string pointId);
    }
}

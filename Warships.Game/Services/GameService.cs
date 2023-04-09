using AutoMapper;
using Warships.Game.Models;
using Warships.Game.Services.Abstract;
using Warships.Setup.Config;
using Warships.Setup.Services.Abstract;

namespace Warships.Game.Services
{
    public class GameService : IGameService
    {
        private readonly GameState _gameState = new();
        private readonly IFleetService _fleetService;
        private readonly BoardDimension _boardDimension = new(10,10);
        private readonly IMapper _mapper;
        public GameService(IFleetService fleetService, IMapper mapper)
        {
            _fleetService = fleetService;
            _mapper = mapper;
        }
        public GameState ShootsFired(string tileId)
        {
            var tile = _gameState.Ships.SelectMany(s => s.Points).Where(t => t.Id == tileId).FirstOrDefault();
            if (tile != null)
            {
                tile.State = PointState.Hit;
                _gameState.Board.Where(t => t.Id == tile.Id).First().State = PointState.Hit;
            }
            else
                _gameState.Board.Where(t => t.Id == tileId).First().State = PointState.Miss;
            return _gameState;
        }

        public GameState StartGame()
        {
            var shipsSetup = _fleetService.BuildFleet();
            var ships = _mapper.Map<List<Ship>>(shipsSetup);
            var gameboard = GenerateBoard(_boardDimension);
            _gameState.Board = gameboard;
            _gameState.Ships = ships;
            return _gameState;
        }

        private static List<Point> GenerateBoard(BoardDimension boardDimension)
        {
            var gameboard = new List<Point>();
            for (int y = 0; y < boardDimension.Height; y++)
            {
                for (int x = 0; x < boardDimension.Width; x++)
                {
                    gameboard.Add(new Point( x, y));
                }
            }
            return gameboard;
        }
    }
}

using AutoMapper;
using Warships.Game.Models;
using Warships.Game.Services.Abstract;
using Warships.Setup.Services.Abstract;

namespace Warships.Game.Services
{
    public class GameService : IGameService
    {
        public readonly GameState GameState = new();
        private readonly IFleetService _fleetService;
        private readonly IBoardGenerator _boardGenerator;
        private readonly IMapper _mapper;
        public GameService(IFleetService fleetService, IBoardGenerator boardGenerator, IMapper mapper)
        {
            _fleetService = fleetService;
            _boardGenerator = boardGenerator;
            _mapper = mapper;
        }
        public GameState ShootsFired(string pointId)
        {
            var point = GameState.Ships.SelectMany(s => s.Points).Where(t => t.Id == pointId).FirstOrDefault();
            if (point != null)
            {
                point.State = PointState.Hit;
                GameState.Board.Where(t => t.Id == point.Id).First().State = PointState.Hit;
            }
            else
                GameState.Board.Where(t => t.Id == pointId).First().State = PointState.Miss;
            return GameState;
        }

        public GameState StartGame()
        {
            var shipsSetup = _fleetService.BuildFleet();
            var ships = _mapper.Map<List<Ship>>(shipsSetup);
            var boardSetup = _boardGenerator.GenerateBoard();
            var gameboard = _mapper.Map<List<Point>>(boardSetup);
            GameState.Board = gameboard;
            GameState.Ships = ships;
            return GameState;
        }
    }
}

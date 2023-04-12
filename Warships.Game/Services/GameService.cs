using AutoMapper;
using Microsoft.Extensions.Options;
using Warships.Configuration;
using Warships.Game.Models;
using Warships.Game.Services.Abstract;
using Warships.Setup.Services.Abstract;

namespace Warships.Game.Services
{
    public class GameService : IGameService
    {
        public GameState GameState { get; internal set; } = new();
        private readonly IFleetService _fleetService;
        private readonly IBoardGenerator _boardGenerator;
        private readonly IMapper _mapper;
        private readonly BoardDimension _boardDimension;
        public GameService(IFleetService fleetService, IBoardGenerator boardGenerator, IMapper mapper, IOptions<BoardDimension> boardDimension)
        {
            _fleetService = fleetService;
            _boardGenerator = boardGenerator;
            _mapper = mapper;
            _boardDimension = boardDimension.Value;
        }
        public GameState ShootsFired(string pointId)
        {
            var point = GameState.Ships.SelectMany(s => s.Points).Where(t => t.Id == pointId).FirstOrDefault();
            if (point != null)
            {
                point.State = PointState.Hit;
                GameState.Board.Points.Where(t => t.Id == point.Id).First().State = PointState.Hit;
            }
            else
                GameState.Board.Points.Where(t => t.Id == pointId).First().State = PointState.Miss;
            return GameState;
        }

        public GameState StartGame()
        {
            var shipsSetup = _fleetService.BuildFleet();
            var ships = _mapper.Map<List<Ship>>(shipsSetup);
            var boardSetup = _boardGenerator.GenerateBoard();
            var points = _mapper.Map<List<Point>>(boardSetup);
            GameState.Board = new Board()
            {
                Dimension = _boardDimension,
                Points = points
            };
            GameState.Ships = ships;
            return GameState;
        }
    }
}

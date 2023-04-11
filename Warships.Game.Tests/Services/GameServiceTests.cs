using AutoMapper;
using Moq;
using System.Text.RegularExpressions;
using Warships.Game.Models.Mapping;
using Warships.Game.Services;
using Warships.Setup.Config;
using Warships.Setup.Services.Abstract;

namespace Warships.Game.Tests.Services
{
    public class GameServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Mock<IFleetService> _fleetServiceMock;
        private readonly Mock<IBoardGenerator> _boardGeneratorMock;
        private readonly List<SetupModels.Ship> _fleet;
        private readonly BoardDimension _BoardDimension = new(10, 10);

        public GameServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            _mapper = mapper;

            _fleet = new List<SetupModels.Ship>()
            {
                new SetupModels.Ship(new List<SetupModels.Point>()
                {
                    new SetupModels.Point(2,2),
                    new SetupModels.Point(2,3),
                    new SetupModels.Point(2,4),
                    new SetupModels.Point(3,5)
                }, "Destroyer"),
                new SetupModels.Ship(new List<SetupModels.Point>()
                {
                    new SetupModels.Point(5,1),
                    new SetupModels.Point(5,2),
                    new SetupModels.Point(5,3),
                    new SetupModels.Point(5,4)
                }, "Destroyer"),
                new SetupModels.Ship(new List<SetupModels.Point>()
                {
                    new SetupModels.Point(7,1),
                    new SetupModels.Point(7,2),
                    new SetupModels.Point(7,3),
                    new SetupModels.Point(7,4),
                    new SetupModels.Point(7,5)
                }, "Battleship")
            };
            _fleetServiceMock = new Mock<IFleetService>();
            _fleetServiceMock.Setup(x => x.BuildFleet())
                .Returns(_fleet);

            _boardGeneratorMock = new Mock<IBoardGenerator>();
            _boardGeneratorMock.Setup(x => x.GenerateBoard())
                .Returns(() => MockBoard());
        }

        private List<SetupModels.Point> MockBoard()
        {
            var board = new List<SetupModels.Point>();
            for (int y = 0; y < _BoardDimension.Height; y++)
            {
                for (int x = 0; x < _BoardDimension.Width; x++)
                {
                    board.Add(new SetupModels.Point(x, y));
                }
            }
            return board;
        }

        [Fact]
        public void StartGame_ShouldReturnValidGameState()
        {
            var gameService = new GameService(_fleetServiceMock.Object, _boardGeneratorMock.Object, _mapper);
            int expectedGameBoardSize = _BoardDimension.Width * _BoardDimension.Height;
            int expectedSunkShips = 0;
            int expectedMissCounter = 0;
            int expectedHitsCounter = 0;
            int expectedShotsFiredCounter = 0;
            var grouppedFleet = _fleet.GroupBy(x => x.Type).ToList();

            Action action = () => gameService.StartGame();

            action.Should().NotThrow();
            gameService.GameState.Should().NotBeNull();
            gameService.GameState.Board.Should().NotBeEmpty();
            gameService.GameState.Board.Should().BeOfType<List<GameModels.Point>>();
            gameService.GameState.Board.Should().HaveCount(expectedGameBoardSize);

            gameService.GameState.Ships.Should().NotBeEmpty();
            gameService.GameState.Ships.Should().BeOfType<List<GameModels.Ship>>();
            gameService.GameState.Ships.Should().HaveCount(_fleet.Count);
            gameService.GameState.Ships.Where(s => s.IsSunk).ToList().Count.Should().Be(expectedSunkShips);

            gameService.GameState.ShipStats.Should().NotBeEmpty();
            gameService.GameState.ShipStats.Should().HaveCount(grouppedFleet.Count);
            foreach (var group in grouppedFleet)
            {
                var stat = gameService.GameState.ShipStats.Where(t => t.Type == group.Key).FirstOrDefault();
                stat.Should().NotBeNull();
                stat?.Type.Should().Be(group.Key);
                stat?.TotalCount.Should().Be(group.Count());
                stat?.LeftInGameCount.Should().Be(group.Count());
            }

            gameService.GameState.Isfinished.Should().BeFalse();
            gameService.GameState.TotalMiss.Should().Be(expectedMissCounter);
            gameService.GameState.TotalHits.Should().Be(expectedHitsCounter);
            gameService.GameState.ShotsFired.Should().Be(expectedShotsFiredCounter);

        }

        [Fact]
        public void ShotsFired_Miss_ShouldAdjustGameState()
        {
            var gameService = new GameService(_fleetServiceMock.Object, _boardGeneratorMock.Object, _mapper);
            var board = MockBoard();
            var gameboard = _mapper.Map<List<GameModels.Point>>(board);
            int expectedMissCounter = 1;
            int expectedHitsCounter = 0;
            int expectedShotsFiredCounter = 1;
            var ship = new GameModels.Ship()
            {
                Points = new List<GameModels.Point>
                {
                    new GameModels.Point(1,1),
                    new GameModels.Point(1,2),
                    new GameModels.Point(1,3),
                    new GameModels.Point(1,4),
                },
                Type = "Destroyer"
            };
            gameService.GameState = new Models.GameState()
            {
                Board = gameboard,
                Ships = new List<GameModels.Ship> { ship }
            };

            Action action = () => gameService.ShootsFired("0.0");

            action.Should().NotThrow();
            gameService.GameState.Isfinished.Should().BeFalse();
            gameService.GameState.TotalMiss.Should().Be(expectedMissCounter);
            gameService.GameState.TotalHits.Should().Be(expectedHitsCounter);
            gameService.GameState.ShotsFired.Should().Be(expectedShotsFiredCounter);

            var gameStateShip = gameService.GameState.Ships.Where(x => x.Type == ship.Type).FirstOrDefault();
            gameStateShip.Should().NotBeNull();
            gameStateShip?.Points.Any(x => x.State != Models.PointState.Unknown).Should().BeFalse();
            gameStateShip?.IsSunk.Should().BeFalse();
        }

        [Fact]
        public void ShotsFired_Hit_ShouldAdjustGameState()
        {
            var gameService = new GameService(_fleetServiceMock.Object, _boardGeneratorMock.Object, _mapper);
            var board = MockBoard();
            var gameboard = _mapper.Map<List<GameModels.Point>>(board);
            int expectedMissCounter = 0;
            int expectedHitsCounter = 1;
            int expectedShotsFiredCounter = 1;
            var ship = new GameModels.Ship()
            {
                Points = new List<GameModels.Point>
                {
                    new GameModels.Point(1,1),
                    new GameModels.Point(1,2),
                    new GameModels.Point(1,3),
                    new GameModels.Point(1,4),
                },
                Type = "Destroyer"
            };
            gameService.GameState = new Models.GameState()
            {
                Board = gameboard,
                Ships = new List<GameModels.Ship> { ship }
            };

            Action action = () => gameService.ShootsFired("1.1");

            action.Should().NotThrow();
            gameService.GameState.Isfinished.Should().BeFalse();
            gameService.GameState.TotalMiss.Should().Be(expectedMissCounter);
            gameService.GameState.TotalHits.Should().Be(expectedHitsCounter);
            gameService.GameState.ShotsFired.Should().Be(expectedShotsFiredCounter);

            var gameStateShip = gameService.GameState.Ships.Where(x => x.Type == ship.Type).FirstOrDefault();
            gameStateShip.Should().NotBeNull();
            gameStateShip?.Points.Where(x => x.State == Models.PointState.Hit).ToList().Count.Should().Be(expectedHitsCounter);
            gameStateShip?.IsSunk.Should().BeFalse();
        }

        [Fact]
        public void ShotsFired_Hit_ShouldSunkShip()
        {
            var gameService = new GameService(_fleetServiceMock.Object, _boardGeneratorMock.Object, _mapper);
            var board = MockBoard();
            var gameboard = _mapper.Map<List<GameModels.Point>>(board);
            string assertShipName = "Destroyer";
            int expectedLeftInGameCount = 0;
            var ships = new List<GameModels.Ship>()
            {
                new GameModels.Ship()
                {
                    Points = new List<GameModels.Point>
                    {
                        new GameModels.Point(1,1) { State = Models.PointState.Hit},
                        new GameModels.Point(1,2) { State = Models.PointState.Hit},
                        new GameModels.Point(1,3) { State = Models.PointState.Hit},
                        new GameModels.Point(1,4),
                    },
                    Type = assertShipName
                },
                new GameModels.Ship()
                {
                    Points = new List<GameModels.Point>
                    {
                        new GameModels.Point(4,1),
                        new GameModels.Point(4,2),
                        new GameModels.Point(4,3),
                        new GameModels.Point(4,4),
                    },
                    Type = "BattleShip"
                }
             };
            gameService.GameState = new Models.GameState()
            {
                Board = gameboard,
                Ships = ships,
            };

            Action action = () => gameService.ShootsFired("1.4");

            action.Should().NotThrow();

            var gameStateShip = gameService.GameState.Ships.Where(x => x.Type == assertShipName).FirstOrDefault();
            gameStateShip.Should().NotBeNull();
            gameStateShip?.Points.Any(x => x.State != Models.PointState.Hit).Should().BeFalse();
            gameStateShip?.IsSunk.Should().BeTrue();

            var stat = gameService.GameState.ShipStats.Where(x => x.Type == assertShipName).ToList().FirstOrDefault();
            stat.Should().NotBeNull();
            stat?.TotalCount.Should().Be(ships.Where(x => x.Type == assertShipName).Count());
            stat?.LeftInGameCount.Should().Be(expectedLeftInGameCount);

            gameService.GameState.Isfinished.Should().BeFalse();
        }

        [Fact]
        public void ShotsFired_Hit_ShouldFinishGame()
        {
            var gameService = new GameService(_fleetServiceMock.Object, _boardGeneratorMock.Object, _mapper);
            var board = MockBoard();
            var gameboard = _mapper.Map<List<GameModels.Point>>(board);
            string assertShipName = "Destroyer";
            int expectedLeftInGameCount = 0;
            var ships = new List<GameModels.Ship>()
            {
                new GameModels.Ship()
                {
                    Points = new List<GameModels.Point>
                    {
                        new GameModels.Point(1,1) { State = Models.PointState.Hit},
                        new GameModels.Point(1,2) { State = Models.PointState.Hit},
                        new GameModels.Point(1,3) { State = Models.PointState.Hit},
                        new GameModels.Point(1,4),
                    },
                    Type = assertShipName
                }
             };
            gameService.GameState = new Models.GameState()
            {
                Board = gameboard,
                Ships = ships,
            };

            Action action = () => gameService.ShootsFired("1.4");

            action.Should().NotThrow();

            var gameStateShip = gameService.GameState.Ships.Where(x => x.Type == assertShipName).FirstOrDefault();
            gameStateShip.Should().NotBeNull();
            gameStateShip?.Points.Any(x => x.State != Models.PointState.Hit).Should().BeFalse();
            gameStateShip?.IsSunk.Should().BeTrue();

            var stat = gameService.GameState.ShipStats.Where(x => x.Type == assertShipName).ToList().FirstOrDefault();
            stat.Should().NotBeNull();
            stat?.TotalCount.Should().Be(ships.Where(x => x.Type == assertShipName).Count());
            stat?.LeftInGameCount.Should().Be(expectedLeftInGameCount);

            gameService.GameState.Isfinished.Should().BeTrue();
        }
    }
}

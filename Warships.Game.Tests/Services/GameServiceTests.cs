using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warships.Game.Models.Mapping;
using Warships.Game.Services;
using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Game.Tests.Services
{
    public class GameServiceTests
    {
        private static IMapper _mapper;

        public GameServiceTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MappingProfile());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public void StartGameTest()
        {
            var fleet = new List<Ship>()
            {
                new Ship(new List<Point>()
                {
                    new Point(2,2),
                    new Point(2,3),
                    new Point(2,4),
                    new Point(3,5)
                }, "Destroyer"),
                new Ship(new List<Point>()
                {
                    new Point(5,1),
                    new Point(5,2),
                    new Point(5,3),
                    new Point(5,4)
                }, "Destroyer"),
                new Ship(new List<Point>()
                {
                    new Point(7,1),
                    new Point(7,2),
                    new Point(7,3),
                    new Point(7,4),
                    new Point(7,5)
                }, "Battleship")
            };
            var fleetServiceMock = new Mock<IFleetService>();
            fleetServiceMock.Setup(x => x.BuildFleet())
                .Returns(fleet);

            var gameService = new GameService(fleetServiceMock.Object, _mapper);

            gameService.StartGame();
        }
    }
}

using Moq;
using System.Collections;
using System.Configuration;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Tests.Services
{
    public class ShipyardTests
    {
        private readonly Mock<IBoardService> _boardServiceMock = new();
        private readonly Mock<IBuildDirectionGenerator> _buildDirectionGeneratorMock = new();
        private BuildDirection _buildDirection;
        private int _getRandomCallCount = 0;
        private int _defineNextPointCallCount = 0;
        public ShipyardTests()
        {
            _boardServiceMock.Setup(x => x.DefineNextPoint(It.IsAny<Point>(), It.IsAny<BuildDirection>()))
                .Returns((Point point, BuildDirection direction) => DefineNextPointMock(point, direction))
                .Callback(() => { _defineNextPointCallCount++; });

            _buildDirectionGeneratorMock.Setup(x => x.GetRandom())
                .Returns(_buildDirection);
        }

        private static Point? DefineNextPointMock(Point point, BuildDirection direction)
        {
            Point nextPoint = point;
            if (direction is BuildDirection.Horizontal)
                nextPoint = new Point(point.X + 1, point.Y);
            else if (direction is BuildDirection.Vertical)
                nextPoint = new Point(point.X, point.Y + 1);

            if (nextPoint.X > 9 || nextPoint.Y > 9)
                return null;
            else
                return nextPoint;
        }

        public class BuildShipTestData
        {
            public Point StartingPoint { get; set; } = new Point(0, 0);
            public BuildDirection BuildDirection { get; set; }
            public int ExpectedSize { get; set; }
            public string Type { get; set; } = string.Empty;
        }

        private class BuildShipTestDataGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new BuildShipTestData()
                    {
                        StartingPoint = new Point(2, 3),
                        BuildDirection = BuildDirection.Horizontal,
                        ExpectedSize = 4,
                        Type = "Destroyer1"
                    }
                };
                yield return new object[]
                {
                    new BuildShipTestData()
                    {
                        StartingPoint = new Point(2, 3),
                        BuildDirection = BuildDirection.Vertical,
                        ExpectedSize = 4,
                        Type = "Destroyer2"
                    }
                };
                yield return new object[]
                {
                    new BuildShipTestData()
                    {
                        StartingPoint = new Point(5, 5),
                        BuildDirection = BuildDirection.Horizontal,
                        ExpectedSize = 5,
                        Type = "Battleship1"
                    }
                };
                yield return new object[]
                {
                    new BuildShipTestData()
                    {
                        StartingPoint = new Point(5, 5),
                        BuildDirection = BuildDirection.Vertical,
                        ExpectedSize = 5,
                        Type = "Battleship2"
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(BuildShipTestDataGenerator))]
        public void BuildShip_ShouldCreateShipFirstAttempt(BuildShipTestData buildShipTestData)
        {
            _boardServiceMock.Setup(x => x.GetRandomAvaiablePoint())
                .Returns(buildShipTestData.StartingPoint)
                .Callback(() => { _getRandomCallCount++; });
            _buildDirection = buildShipTestData.BuildDirection;
            
            Ship? ship = null;
            const int expectedRandomPointFetchCount = 1;
            var shipyard = new Shipyard(_boardServiceMock.Object, _buildDirectionGeneratorMock.Object);

            Action action = () => ship = shipyard.BuildShip(buildShipTestData.Type, buildShipTestData.ExpectedSize);

            action.Should().NotThrow();
            ship.Should().NotBeNull();
            ship?.Points.Should().NotBeNull().And.HaveCount(buildShipTestData.ExpectedSize);
            ship?.Type.Should().Be(buildShipTestData.Type);
            _getRandomCallCount.Should().Be(expectedRandomPointFetchCount);
            _defineNextPointCallCount.Should().Be(buildShipTestData.ExpectedSize - 1);
        }

        [Fact]
        public void BuildShip_CircuitBreakerShouldThrow()
        {
            BuildShipTestData buildShipTestData = new BuildShipTestData()
            {
                StartingPoint = new Point(10, 10),
                BuildDirection = BuildDirection.Horizontal,
                Type = "ImpossibleShip",
                ExpectedSize = 4,
            };
            _boardServiceMock.Setup(x => x.GetRandomAvaiablePoint())
                .Returns(buildShipTestData.StartingPoint);
            _buildDirection = buildShipTestData.BuildDirection;

            Ship? ship = null;
            var shipyard = new Shipyard(_boardServiceMock.Object, _buildDirectionGeneratorMock.Object);

            Action action = () => ship = shipyard.BuildShip(buildShipTestData.Type, buildShipTestData.ExpectedSize);

            action.Should().ThrowExactly<ConfigurationErrorsException>().WithMessage(ExceptionMessages.BuildShipCircuitBreakerException);
        }

        [Theory]
        [ClassData(typeof(BuildShipTestDataGenerator))]
        public void BuildShip_ShouldCreateShipMultipleAttempts(BuildShipTestData buildShipTestData)
        {
            var sequence = new MockSequence();
            _boardServiceMock.InSequence(sequence).Setup(x => x.GetRandomAvaiablePoint())
                .Returns(new Point(8,8))
                .Callback(() => { _getRandomCallCount++; });
            _boardServiceMock.InSequence(sequence).Setup(x => x.GetRandomAvaiablePoint())
                .Returns(new Point(9, 9))
                .Callback(() => { _getRandomCallCount++; });
            _boardServiceMock.InSequence(sequence).Setup(x => x.GetRandomAvaiablePoint())
                .Returns(buildShipTestData.StartingPoint)
                .Callback(() => { _getRandomCallCount++; });
            _buildDirection = buildShipTestData.BuildDirection;

            Ship? ship = null;
            const int expectedRandomPointFetchCount = 3;
            var shipyard = new Shipyard(_boardServiceMock.Object, _buildDirectionGeneratorMock.Object);

            Action action = () => ship = shipyard.BuildShip(buildShipTestData.Type, buildShipTestData.ExpectedSize);

            action.Should().NotThrow();
            ship.Should().NotBeNull();
            ship?.Points.Should().NotBeNull().And.HaveCount(buildShipTestData.ExpectedSize);
            ship?.Type.Should().Be(buildShipTestData.Type);
            _getRandomCallCount.Should().Be(expectedRandomPointFetchCount);
        }
    }
}

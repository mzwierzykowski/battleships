using Microsoft.Extensions.Options;
using Moq;
using System.Collections;
using System.Configuration;
using Warships.Setup.Config;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Tests.Services
{
    public class FleetServiceTests
    {
        private readonly Mock<IBoardService> _boardServiceMock = new();
        private readonly Mock<IShipyard> _shipyardMock = new();

        public FleetServiceTests()
        {
            _shipyardMock.Setup(x => x.BuildShip(It.IsAny<string>(), It.IsAny<int>()))
                .Returns((string type, int size) =>
                {
                    return new Ship(new List<Point>(), type);
                });
        }
        private class FleetConfigurationErrorGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new FleetConfiguration()
                    {
                        Blueprints = null
                    }
                };
                yield return new object[]
                {
                    new FleetConfiguration()
                    {
                        Blueprints = new()
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        [Theory]
        [InlineData(null)]
        [ClassData(typeof(FleetConfigurationErrorGenerator))]
        public void BuildFleet_ConfigurationError_ShouldThrow(FleetConfiguration fleetConfiguration)
        {
            List<Ship> fleet = new();
            var fleetConfigurationOptions = Options.Create<FleetConfiguration>(fleetConfiguration);
            var shipyard = new FleetService(fleetConfigurationOptions, _boardServiceMock.Object, _shipyardMock.Object);
            Action action = () => fleet = shipyard.BuildFleet();

            action.Should().ThrowExactly<ConfigurationErrorsException>().WithMessage(ExceptionMessages.MissingFleetConfiguration);
        }
        private class FleetConfigurationGenerator : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[]
                {
                    new FleetConfiguration()
                    {
                        Blueprints = new List<ShipBlueprint>
                        {
                            new ShipBlueprint()
                            {
                                Type = "Destroyer",
                                Size = 4,
                                Count = 1
                            }
                        }
                    }
                };
                yield return new object[]
                {
                    new FleetConfiguration()
                    {
                        Blueprints = new List<ShipBlueprint>
                        {
                            new ShipBlueprint()
                            {
                                Type = "Destroyer",
                                Size = 4,
                                Count = 2
                            },
                            new ShipBlueprint()
                            {
                                Type = "Battleship",
                                Size = 5,
                                Count = 1
                            }
                        }
                    }
                };
            }
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Theory]
        [ClassData(typeof(FleetConfigurationGenerator))]
        public void BuildFleet_ShouldBuildFleet(FleetConfiguration fleetConfiguration)
        {
            List<Ship>? fleet = null;
            var fleetConfigurationOptions = Options.Create(fleetConfiguration);
            int expectedFleetCount = 0;
            if (fleetConfiguration != null && fleetConfiguration.Blueprints != null)
            {
                expectedFleetCount = fleetConfiguration.Blueprints.Sum(s => s.Count);
            }

            var shipyard = new FleetService(fleetConfigurationOptions, _boardServiceMock.Object, _shipyardMock.Object);


            Action action = () => fleet = shipyard.BuildFleet();

            action.Should().NotThrow();
            fleet.Should().NotBeNull();
            fleet?.Should().HaveCount(expectedFleetCount);
        }
    }
}

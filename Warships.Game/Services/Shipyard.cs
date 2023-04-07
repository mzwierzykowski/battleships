using Microsoft.Extensions.Options;
using System.Configuration;
using Warships.Setup.Config;
using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Services
{
    public class Shipyard : IShipyard
    {
        private readonly IBoardService _boardService;
        private readonly IBuildDirectionGenerator _buildDirectionGenerator;
        
        public Shipyard(IOptions<FleetConfiguration> fleetBlueprints, IBoardService boardService, IBuildDirectionGenerator buildDirectionGenerator)
        {
            _boardService = boardService;
            _buildDirectionGenerator = buildDirectionGenerator;
        }

        public virtual Ship BuildShip(string type, int size)
        {
            var buildDirection = _buildDirectionGenerator.GetRandom();
            var currentPoint = _boardService.GetRandomAvaiablePoint();
            var pointsList = new List<Point>() { currentPoint };
            int attempts = 0;
            const int maxAttempts = 100;
            do
            {
                var nextPoint = _boardService.DefineNextPoint(currentPoint, buildDirection);
                if (nextPoint != null)
                {
                    pointsList.Add(nextPoint);
                    currentPoint = nextPoint;
                }
                else
                {
                    pointsList.Clear();
                    currentPoint = _boardService.GetRandomAvaiablePoint();
                    pointsList.Add(currentPoint);
                }
                attempts++;
            } while (pointsList.Count != size && attempts != maxAttempts);

            if (attempts == maxAttempts)
                throw new ConfigurationErrorsException(ExceptionMessages.BuildShipCircuitBreakerException);

            var ship = new Ship(pointsList, type);
            return ship;
        }
    }
}

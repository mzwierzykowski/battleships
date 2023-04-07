﻿using Microsoft.Extensions.Options;
using System.Configuration;
using Warships.Setup.Config;
using Warships.Setup.Models;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.Services
{
    public class FleetService
    {
        private readonly FleetConfiguration _fleetConfiguration;
        private readonly BoardDimension boardDimension = new(10, 10);
        private readonly IBoardService _boardService;
        private readonly IShipyard _shipyard;
        public FleetService(IOptions<FleetConfiguration> fleetBlueprints, IBoardService boardService, IShipyard shipyard)
        {
            _fleetConfiguration = fleetBlueprints.Value;
            _boardService = boardService;
            _shipyard = shipyard;
        }
        public List<Ship> BuildFleet()
        {
            if (_fleetConfiguration?.Blueprints == null || !_fleetConfiguration.Blueprints.Any())
                throw new ConfigurationErrorsException(ExceptionMessages.MissingFleetConfiguration);

            var fleet = new List<Ship>();
            _boardService.GenerateBoard(boardDimension);
            foreach (var shipConfig in _fleetConfiguration.Blueprints)
            {
                for (int i = 0; i < shipConfig.Count; i++)
                {
                    var ship = _shipyard.BuildShip(shipConfig.Type, shipConfig.Size);
                    fleet.Add(ship);
                    _boardService.RemoveOccupiedArea(ship);
                }
            }
            return fleet;
        }
    }
}

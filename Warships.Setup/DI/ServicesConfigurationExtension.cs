using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Warships.Setup.Config;
using Warships.Setup.Models;
using Warships.Setup.Services;
using Warships.Setup.Services.Abstract;

namespace Warships.Setup.DI
{
    [ExcludeFromCodeCoverage]
    public static class ServicesConfigurationExtension
    {
        public static void AddSetupServices(this IServiceCollection services, IConfiguration configuration)
        {
            var fleetConfiguration = configuration.GetSection("FleetConfiguration");
            services.Configure<FleetConfiguration>(fleetConfiguration);
            var boardDimension = configuration.GetSection("BoardDimension");
            services.Configure<BoardDimension>(boardDimension);
            services.AddSingleton<IBoardService, BoardService>();
            services.AddSingleton<IBuildDirectionGenerator, BuildDirectionGenerator>();
            services.AddSingleton<IShipyard, Shipyard>();
            services.AddSingleton<IFleetService, FleetService>();
            services.AddSingleton<IBoardGenerator, BoardGenerator>();
        }
    }
}

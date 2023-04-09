using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using Warships.Game.Models.Mapping;
using Warships.Game.Services;
using Warships.Game.Services.Abstract;
using Warships.Setup.DI;

namespace Warships.Game.DI
{
    [ExcludeFromCodeCoverage]
    public static class ServicesConfigurationExtension
    {
        public static void AddGameServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSetupServices(configuration);
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddSingleton<IGameService, GameService>();
        }
    }
}

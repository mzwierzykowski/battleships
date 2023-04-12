using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Warships.API.Models.Mapping
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game.Models.GameState, GameState>();
        }
    }
}

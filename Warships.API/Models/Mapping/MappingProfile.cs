using AutoMapper;

namespace Warships.API.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game.Models.GameState, GameState>();
        }
    }
}

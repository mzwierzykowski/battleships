using AutoMapper;
using Warships.Setup.Models;

namespace Warships.Game.Models.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Setup.Models.Point, Point>();
            CreateMap<Setup.Models.Ship, Ship>();
        }
    }
}

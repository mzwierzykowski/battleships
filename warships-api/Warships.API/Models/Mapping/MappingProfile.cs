using AutoMapper;
using System.Diagnostics.CodeAnalysis;

namespace Warships.API.Models.Mapping
{
    [ExcludeFromCodeCoverage]
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GameState, Warships.Game.Models.GameState>().ReverseMap()
                .ForPath(d => d.Stats, o => o.MapFrom(s => new Stats()
                {
                    ShipStats = s.ShipStats,
                    TotalHits = s.TotalHits,
                    TotalMiss = s.TotalMiss,
                    ShotsFired = s.ShotsFired,
                }));
        }
    }
}

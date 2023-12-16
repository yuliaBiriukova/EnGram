using AutoMapper;
using CatalogService.Features.Levels;
using EnGram.DB.Entities;

namespace CatalogService.Infrastructure.Mapping;

public class LevelProfile : Profile
{
    public LevelProfile()
    {
        CreateMap<LevelAddCommand, Level>();
        CreateMap<Level, LevelViewModel>();
        CreateMap<LevelUpdateModel, Level>();
    }
}
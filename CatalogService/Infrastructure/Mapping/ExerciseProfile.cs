using AutoMapper;
using CatalogService.Features.Exercises;
using EnGram.DB.Entities;

namespace CatalogService.Infrastructure.Mapping;

public class ExerciseProfile : Profile
{
    public ExerciseProfile()
    {
        CreateMap<Exercise, ExerciseViewModel>();
        CreateMap<ExerciseAddCommand, Exercise>();
        CreateMap<ExerciseUpdateViewModel, Exercise>();
    }
}
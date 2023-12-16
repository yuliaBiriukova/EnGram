using AutoMapper;
using CatalogService.Features.Topics;
using EnGram.DB.Entities;

namespace CatalogService.Infrastructure.Mapping;

public class TopicProfile : Profile
{
    public TopicProfile()
    {
        CreateMap<Topic, TopicViewModel>();
        CreateMap<TopicAddCommand, Topic>();
        CreateMap<TopicUpdateModel, Topic>();
    }
}
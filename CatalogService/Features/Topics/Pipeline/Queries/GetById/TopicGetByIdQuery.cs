using AutoMapper;
using EnGram.DB.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Topics;

public record TopicGetByIdQuery(int Id) : IRequest<TopicViewModel>;

public class TopicGetByIdQueryHandler : IRequestHandler<TopicGetByIdQuery, TopicViewModel>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public TopicGetByIdQueryHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<TopicViewModel> Handle(TopicGetByIdQuery request, CancellationToken cancellationToken)
    {
        var topic = await _dbContext.Topics.FindAsync(request.Id);
        return _mapper.Map<TopicViewModel>(topic);
    }
}

public class TopicGetByIdQueryValidator : AbstractValidator<TopicGetByIdQuery>
{
    private readonly EnGramDbContext _dbContext;

    public TopicGetByIdQueryValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(CheckTopicExists)
            .WithMessage(x => GetTopicNotExistsMessage(x.Id));
    }

    private async Task<bool> CheckTopicExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Topics.AnyAsync(t => t.Id == id);
    }

    private static string GetTopicNotExistsMessage(int id) =>
        $"Topic with id={id} does not exist";
}
using AutoMapper;
using EnGram.DB.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Topics;

public record TopicSearchQuery(TopicFilter Filter) : IRequest<IEnumerable<TopicViewModel>>;

public class TopicSearchQueryQueryHandler : IRequestHandler<TopicSearchQuery, IEnumerable<TopicViewModel>>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public TopicSearchQueryQueryHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<TopicViewModel>> Handle(TopicSearchQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Topics.AsQueryable();

        if(request.Filter.LevelId is not null)
        {
            query = query.Where(t => t.LevelId == request.Filter.LevelId);
        }

        if(request.Filter.Name is not null)
        {
            var searchTerms = request.Filter.Name.Split(' ');

            foreach (var term in searchTerms)
            {
                query = query.Where(t => t.Name.Contains(term));
            }
        }

        var topics = await query.ToListAsync();

        return _mapper.Map<IEnumerable<TopicViewModel>>(topics);
    }
}

public class TopicSearchQueryQueryValidator : AbstractValidator<TopicSearchQuery>
{
    private readonly EnGramDbContext _dbContext;

    public TopicSearchQueryQueryValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Filter.LevelId)
            .MustAsync(ValidateLevelId)
            .WithMessage(x => GetLevelNotExistsMessage(x.Filter.LevelId));
    }

    private async Task<bool> ValidateLevelId(int? id, CancellationToken cancellationToken)
    {
        if(id.HasValue)
        {
            return await _dbContext.Levels.AnyAsync(l => l.Id == id);
        }
        return true;
    }

    private static string GetLevelNotExistsMessage(int? id) =>
        $"Level with id={id} does not exist";
}
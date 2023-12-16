using AutoMapper;
using EnGram.DB.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Exercises;

public record ExerciseGetByTopicIdQuery(int TopicId) : IRequest<IEnumerable<ExerciseViewModel>>;

public class ExerciseGetByTopicIdQueryHandler : IRequestHandler<ExerciseGetByTopicIdQuery, IEnumerable<ExerciseViewModel>>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public ExerciseGetByTopicIdQueryHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<ExerciseViewModel>> Handle(ExerciseGetByTopicIdQuery request, CancellationToken cancellationToken)
    {
        var exercises = await _dbContext.Exercises
            .Where(e => e.TopicId == request.TopicId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<ExerciseViewModel>>(exercises);
    }
}

public class ExerciseGetByTopicIdQueryValidator : AbstractValidator<ExerciseGetByTopicIdQuery>
{
    private readonly EnGramDbContext _dbContext;

    public ExerciseGetByTopicIdQueryValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.TopicId)
            .MustAsync(CheckTopicExists)
            .WithMessage(x => GetTopicNotExistsMessage(x.TopicId));
    }

    private async Task<bool> CheckTopicExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Topics.AnyAsync(t => t.Id == id);
    }

    private static string GetTopicNotExistsMessage(int id) =>
        $"Topic with id={id} does not exist";
}
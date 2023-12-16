using AutoMapper;
using EnGram.DB.Database;
using EnGram.DB.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Topics;

public record TopicAddCommand(
    int LevelId,
    string Name,
    string Content) : IRequest<int>;

public class TopicAddCommandHandler : IRequestHandler<TopicAddCommand, int>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public TopicAddCommandHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<int> Handle(TopicAddCommand request, CancellationToken cancellationToken)
    {
        var newTopic = _mapper.Map<Topic>(request);
        _dbContext.Topics.Add(newTopic);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newTopic.Id;
    }
}

public class TopicAddCommandValidator : AbstractValidator<TopicAddCommand>
{
    private readonly EnGramDbContext _dbContext;

    public TopicAddCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(128);

        RuleFor(x => x.Content)
            .MinimumLength(1);

        RuleFor(x => x.LevelId)
          .MustAsync(CheckLevelExists)
          .WithMessage(x => GetLevelNotExistsMessage(x.LevelId));
    }

    private async Task<bool> CheckLevelExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Levels.AnyAsync(l => l.Id == id);
    }

    private static string GetLevelNotExistsMessage(int id) =>
        $"Level with id={id} does not exist";
}
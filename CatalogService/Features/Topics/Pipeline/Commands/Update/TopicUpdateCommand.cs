using AutoMapper;
using EnGram.DB.Database;
using EnGram.DB.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Topics;

public record TopicUpdateCommand(
    int Id,
    TopicUpdateModel Model) : IRequest<bool>;

public class TopicUpdateCommandHandler : IRequestHandler<TopicUpdateCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public TopicUpdateCommandHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(TopicUpdateCommand request, CancellationToken cancellationToken)
    {
        var topicToUpdate = _mapper.Map<Topic>(request.Model);
        topicToUpdate.Id = request.Id;
        _dbContext.Topics.Update(topicToUpdate);
        var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
        return affectedRows > 0;
    }
}

public class TopicUpdateCommandValidator : AbstractValidator<TopicUpdateCommand>
{
    private readonly EnGramDbContext _dbContext;

    public TopicUpdateCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(CheckTopicExists)
            .WithMessage(x => GetEntityNotExistsMessage(x.Id, typeof(Topic).Name));

        RuleFor(x => x.Model.LevelId)
            .MustAsync(CheckLevelExists)
            .WithMessage(x => GetEntityNotExistsMessage(x.Model.LevelId, typeof(Level).Name));

        RuleFor(x => x.Model.Name)
            .MinimumLength(1)
            .MaximumLength(128);

        RuleFor(x => x.Model.Content)
            .MinimumLength(1);
    }

    private async Task<bool> CheckTopicExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Topics.AnyAsync(t => t.Id == id);
    }

    private async Task<bool> CheckLevelExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Levels.AnyAsync(l => l.Id == id);
    }

    private static string GetEntityNotExistsMessage(int id, string entityName) =>
        $"{entityName} with id={id} does not exist";
}
using EnGram.DB.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Topics;

public record TopicDeleteCommand(int Id) : IRequest<bool>;

public class TopicDeleteCommandHandler : IRequestHandler<TopicDeleteCommand, bool>
{
    private readonly EnGramDbContext _dbContext;

    public TopicDeleteCommandHandler(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(TopicDeleteCommand request, CancellationToken cancellationToken)
    {
        var topicToDelete = await _dbContext.Topics.FindAsync(request.Id);

        if (topicToDelete is not null)
        {
            _dbContext.Topics.Remove(topicToDelete);
            var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
            return affectedRows > 0;
        }

        return false;
    }
}

public class TopicDeleteCommandValidator : AbstractValidator<TopicDeleteCommand>
{
    private readonly EnGramDbContext _dbContext;

    public TopicDeleteCommandValidator(EnGramDbContext dbContext)
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
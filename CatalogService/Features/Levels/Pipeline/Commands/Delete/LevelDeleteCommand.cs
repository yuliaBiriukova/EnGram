using EnGram.DB.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Levels;

public record LevelDeleteCommand(int Id) : IRequest<bool>;

public class LevelDeleteCommandHandler : IRequestHandler<LevelDeleteCommand, bool>
{
    private readonly EnGramDbContext _dbContext;

    public LevelDeleteCommandHandler(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(LevelDeleteCommand request, CancellationToken cancellationToken)
    {
        var levelToDelete = await _dbContext.Levels.FindAsync(request.Id);

        if (levelToDelete is not null)
        {
            _dbContext.Levels.Remove(levelToDelete);
            var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
            return affectedRows > 0;
        }

        return false;
    }
}

public class LevelDeleteCommandValidator : AbstractValidator<LevelDeleteCommand>
{
    private readonly EnGramDbContext _dbContext;

    public LevelDeleteCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(CheckLevelExists)
            .WithMessage(x => GetLevelNotExistsMessage(x.Id));
    }

    private async Task<bool> CheckLevelExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Levels.AnyAsync(l => l.Id == id);
    }

    private static string GetLevelNotExistsMessage(int id) =>
        $"Level with id={id} does not exist";
}
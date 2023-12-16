using CatalogService.Features.Topics;
using EnGram.DB.Database;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Exercises;

public record ExerciseDeleteCommand(int Id) : IRequest<bool>;

public class ExerciseDeleteCommandHandler : IRequestHandler<ExerciseDeleteCommand, bool>
{
    private readonly EnGramDbContext _dbContext;

    public ExerciseDeleteCommandHandler(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ExerciseDeleteCommand request, CancellationToken cancellationToken)
    {
        var exerciseToDelete = await _dbContext.Exercises.FindAsync(request.Id);

        if (exerciseToDelete is not null)
        {
            _dbContext.Exercises.Remove(exerciseToDelete);
            var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
            return affectedRows > 0;
        }

        return false;
    }
}

public class ExerciseDeleteCommandValidator : AbstractValidator<ExerciseDeleteCommand>
{
    private readonly EnGramDbContext _dbContext;

    public ExerciseDeleteCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(CheckExerciseExists)
            .WithMessage(x => GetExerciseNotExistsMessage(x.Id));
    }

    private async Task<bool> CheckExerciseExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Exercises.AnyAsync(t => t.Id == id);
    }

    private static string GetExerciseNotExistsMessage(int id) =>
        $"Exercise with id={id} does not exist";
}
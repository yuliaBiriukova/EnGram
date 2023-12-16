using AutoMapper;
using EnGram.DB.Database;
using EnGram.DB.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Exercises;

public record ExerciseUpdateCommand(
    int Id,
    ExerciseUpdateViewModel Model) : IRequest<bool>;

public class ExerciseUpdateCommandHandler : IRequestHandler<ExerciseUpdateCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public ExerciseUpdateCommandHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(ExerciseUpdateCommand request, CancellationToken cancellationToken)
    {
        var exerciseToUpdate = _mapper.Map<Exercise>(request.Model);
        exerciseToUpdate.Id = request.Id;
        _dbContext.Exercises.Update(exerciseToUpdate);
        var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
        return affectedRows > 0;
    }
}

public class ExerciseUpdateCommandValidator : AbstractValidator<ExerciseUpdateCommand>
{
    private readonly EnGramDbContext _dbContext;

    public ExerciseUpdateCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(CheckExerciseExists)
            .WithMessage(x => GetEntityNotExistsMessage(x.Id, typeof(Exercise).Name));

        RuleFor(x => x.Model.Task)
            .MinimumLength(1)
            .MaximumLength(256);

        RuleFor(x => x.Model.Answer)
           .MinimumLength(1)
           .MaximumLength(256);

        RuleFor(x => x.Model.TopicId)
            .MustAsync(CheckTopicExists)
            .WithMessage(x => GetEntityNotExistsMessage(x.Id, typeof(Topic).Name));
    }

    private async Task<bool> CheckTopicExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Topics.AnyAsync(t => t.Id == id);
    }

    private async Task<bool> CheckExerciseExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Exercises.AnyAsync(t => t.Id == id);
    }

    private static string GetEntityNotExistsMessage(int id, string entityName) =>
        $"{entityName} with id={id} does not exist";
}
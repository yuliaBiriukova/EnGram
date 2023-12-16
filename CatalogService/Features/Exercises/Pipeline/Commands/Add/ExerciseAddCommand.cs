using AutoMapper;
using EnGram.DB.Database;
using EnGram.DB.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Exercises;

public record ExerciseAddCommand(
    int TopicId,
    ExerciseType Type,
    string Task,
    string Answer) : IRequest<int>;

public class ExerciseAddCommandHandler : IRequestHandler<ExerciseAddCommand, int>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public ExerciseAddCommandHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<int> Handle(ExerciseAddCommand request, CancellationToken cancellationToken)
    {
        var newExercise = _mapper.Map<Exercise>(request);
        _dbContext.Exercises.Add(newExercise);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newExercise.Id;
    }
}

public class ExerciseAddCommandValidator : AbstractValidator<ExerciseAddCommand>
{
    private readonly EnGramDbContext _dbContext;

    public ExerciseAddCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Task)
            .MinimumLength(1)
            .MaximumLength(256);

        RuleFor(x => x.Answer)
           .MinimumLength(1)
           .MaximumLength(256);

        RuleFor(x => x.TopicId)
            .MustAsync(CheckTopicExists)
            .WithMessage(x => GetGetTopicNotExistsMessage(x.TopicId));
    }

    private async Task<bool> CheckTopicExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Topics.AnyAsync(t => t.Id == id);
    }

    private static string GetGetTopicNotExistsMessage(int id) =>
        $"Topic with id={id} does not exist";
}

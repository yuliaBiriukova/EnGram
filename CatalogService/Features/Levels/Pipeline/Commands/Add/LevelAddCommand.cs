using AutoMapper;
using EnGram.DB.Database;
using EnGram.DB.Entities;
using FluentValidation;
using MediatR;

namespace CatalogService.Features.Levels;

public record LevelAddCommand(
    string Code, 
    string Name) : IRequest<int>;

public class LevelAddCommandHandler : IRequestHandler<LevelAddCommand, int>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public LevelAddCommandHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<int> Handle(LevelAddCommand request, CancellationToken cancellationToken)
    {
        var newLevel = _mapper.Map<Level>(request);
        _dbContext.Levels.Add(newLevel);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return newLevel.Id;
    }
}

public class LevelAddCommandValidator : AbstractValidator<LevelAddCommand>
{
    public LevelAddCommandValidator()
    {
        RuleFor(x => x.Code)
            .MinimumLength(1)
            .MaximumLength(10);

        RuleFor(x => x.Name)
            .MinimumLength(1)
            .MaximumLength(100);
    }
}
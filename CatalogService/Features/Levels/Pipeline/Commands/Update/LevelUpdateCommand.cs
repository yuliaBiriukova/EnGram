using AutoMapper;
using EnGram.DB.Database;
using EnGram.DB.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Levels;

public record LevelUpdateCommand(
    int Id,
    LevelUpdateModel Model) : IRequest<bool>;

public class LevelUpdateCommandHandler : IRequestHandler<LevelUpdateCommand, bool>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public LevelUpdateCommandHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(LevelUpdateCommand request, CancellationToken cancellationToken)
    {
        var levelToUpdate = _mapper.Map<Level>(request.Model);
        levelToUpdate.Id = request.Id;
        _dbContext.Levels.Update(levelToUpdate);
        var affectedRows = await _dbContext.SaveChangesAsync(cancellationToken);
        return affectedRows > 0;
    }
}

public class LevelUpdateCommandValidator : AbstractValidator<LevelUpdateCommand>
{
    private readonly EnGramDbContext _dbContext;

    public LevelUpdateCommandValidator(EnGramDbContext dbContext)
    {
        _dbContext = dbContext;

        RuleFor(x => x.Id)
            .MustAsync(CheckLevelExists)
            .WithMessage(x => GetLevelNotExistsMessage(x.Id));

        RuleFor(x => x.Model.Code)
            .MinimumLength(1)
            .MaximumLength(10);

        RuleFor(x => x.Model.Name)
            .MinimumLength(1)
            .MaximumLength(100);
    }

    private async Task<bool> CheckLevelExists(int id, CancellationToken cancellationToken)
    {
        return await _dbContext.Levels.AnyAsync(l => l.Id == id);
    }

    private static string GetLevelNotExistsMessage(int id) => 
        $"Level with id={id} does not exist";
}
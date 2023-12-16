using AutoMapper;
using EnGram.DB.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Levels;

public record LevelsGetAllQuery() : IRequest<IEnumerable<LevelViewModel>>;

public class LevelsGetAllQueryyHandler : IRequestHandler<LevelsGetAllQuery, IEnumerable<LevelViewModel>>
{
    private readonly IMapper _mapper;
    private readonly EnGramDbContext _dbContext;

    public LevelsGetAllQueryyHandler(IMapper mapper, EnGramDbContext dbContext)
    {
        _mapper = mapper;
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<LevelViewModel>> Handle(LevelsGetAllQuery request, CancellationToken cancellationToken)
    {
        var levels = await _dbContext.Levels.ToListAsync();
        return _mapper.Map<IEnumerable<LevelViewModel>>(levels);
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Features.Levels;

[Route("api/[controller]")]
[ApiController]
public class LevelsController : ControllerBase
{
    private readonly IMediator _mediator;

    public LevelsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var command = new LevelsGetAllQuery();
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] LevelAddCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LevelUpdateModel model)
    {
        var command = new LevelUpdateCommand(id, model);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new LevelDeleteCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
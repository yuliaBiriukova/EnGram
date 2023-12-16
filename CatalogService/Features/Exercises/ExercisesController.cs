using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Features.Exercises;

[Route("api/[controller]")]
[ApiController]
public class ExercisesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExercisesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{topicId}")]
    public async Task<IActionResult> GetByTopicId(int topicId)
    {
        var query = new ExerciseGetByTopicIdQuery(topicId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] ExerciseAddCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> pdate(int id, [FromBody] ExerciseUpdateViewModel model)
    {
        var command = new ExerciseUpdateCommand(id, model);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new ExerciseDeleteCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
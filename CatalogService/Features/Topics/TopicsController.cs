using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Features.Topics;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TopicsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetByFilter([FromQuery] TopicFilter filter)
    {
        var query = new TopicSearchQuery(filter);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new TopicGetByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] TopicAddCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TopicUpdateModel model)
    {
        var command = new TopicUpdateCommand(id, model);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var command = new TopicDeleteCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
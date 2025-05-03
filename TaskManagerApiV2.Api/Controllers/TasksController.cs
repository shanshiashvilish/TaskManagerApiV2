using Microsoft.AspNetCore.Mvc;
using TaskManagerApiV2.Domain.Abstractions;

namespace TaskManagerApiV2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] string title)
    {
        try
        {
            var task = await taskService.CreateAsync(title);
            return Ok(task);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetTaskHistory([FromRoute] Guid id)
    {
        var history = await taskService.GetTaskHistoryAsync(id);
        return Ok(history);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await taskService.GetAllAsync();
        return Ok(tasks);
    }
}
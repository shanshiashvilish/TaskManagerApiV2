using Microsoft.AspNetCore.Mvc;
using TaskManagerApiV2.Domain.Abstractions;

namespace TaskManagerApiV2.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] string username)
    {
        try
        {
            var user = await userService.CreateAsync(username);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }
}
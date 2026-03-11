using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    public readonly IUserService _userService;
    public readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        var createdUser = await _userService.CreateUser(user);
          _logger.LogInformation("User created:{UserId}" , createdUser.Id);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllUsers()
    {
        var getAllUsers = await _userService.GetAllUsers();
        _logger.LogInformation("Get all users");
        return Ok(getAllUsers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetUser(int id)
    {
        var getById = await _userService.GetUserById(id);
        _logger.LogInformation("Get user with id:{UserId}", id);
        return Ok(getById);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateUser(int id, [FromBody] User user)
    {
        var update = await _userService.UpdateUser(id, user);
        _logger.LogInformation("Update user with id:{id}", id);
        return Ok(update);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);
        _logger.LogInformation("Delete user with id:{id}", id);
        return NoContent();
    }
}
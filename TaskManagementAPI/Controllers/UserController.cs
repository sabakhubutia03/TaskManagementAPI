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
    public async Task<IActionResult> CreateUser([FromBody] User user)
    {
        var createdUser = await _userService.CreateUser(user);
          _logger.LogInformation("User created:{createdUser.Id}" , createdUser.Id);
        return Ok(createdUser);
    }

    [HttpGet]
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var getAllUsers = await _userService.GetAllUsers();
        _logger.LogInformation("Get all users");
        return getAllUsers;
    }
}
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services;

namespace TaskManagementAPI.Controllers;
[ApiController]
[Route("api/[controller]")]
public class TaskController : ControllerBase
{
    public readonly ITaskService _taskService;
    public readonly ILogger<TaskController> _logger;

    public TaskController(ITaskService taskService, ILogger<TaskController> logger)
    {
        _taskService = taskService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask(TaskItem task)
    {
        var createTask = await _taskService.CreateTask(task);
        _logger.LogInformation("Task created: {createTask.Id}",createTask.Id);
        return CreatedAtAction(nameof(CreateTask), new { id = createTask.Id }, createTask);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTasks()
    {
        var getAllTasks = await _taskService.GetAllTasks();
        _logger.LogInformation("Get All Tasks");
        return Ok(getAllTasks);
    }
}
using Microsoft.AspNetCore.Mvc;
using TaskManagementAPI.Application.Interfaces; 
using TaskManagementAPI.Domain.Entities;

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
    public async Task<ActionResult> CreateTask(TaskItem task)
    {
        var createTask = await _taskService.CreateTask(task);
        _logger.LogInformation("Task created: {createTask.Id}",createTask.Id);
        return CreatedAtAction(nameof(GetTaskById), new { id = createTask.Id }, createTask);
    }

    [HttpGet]
    public async Task<ActionResult> GetAllTasks()
    {
        var getAllTasks = await _taskService.GetAllTasks();
        _logger.LogInformation("Get All Tasks");
        return Ok(getAllTasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetTaskById(int id)
    {
        var getByTaskId = await _taskService.GetTaskById(id);
        _logger.LogInformation("Get Task ById {id}", id);
        return Ok(getByTaskId);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTask(int id, TaskItem task)
    {
        var updateTask = await _taskService.UpdateTask(id, task);
        _logger.LogInformation("Update Task {id}", id);
        return Ok(updateTask);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTask(int id)
    {
        await _taskService.DeleteTask(id);
        _logger.LogInformation("Delete Task {id}", id);
        return NoContent();
    }
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasksByUserId(int userId)
    {
        var tasks = await _taskService.GetTasksByUserId(userId);
        _logger.LogInformation("Retrieved tasks for user {UserId}", userId);
        return Ok(tasks);
    }
    
}
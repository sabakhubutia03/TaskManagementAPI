using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Infrastructure.Data;

public class TaskService : ITaskService
{ 
    public readonly ApplicationDbContext _db;
    public readonly ILogger<TaskService> _logger;

    public TaskService(ApplicationDbContext db, ILogger<TaskService> logger)
    {
        _db = db;
        _logger = logger;
    }
    
    public async Task<IEnumerable<TaskItem>> GetAllTasks()
       => await _db.TaskItems.ToListAsync();
    

    public  async Task<TaskItem> GetTaskById(int id)
    {
        var task = await _db.TaskItems.FindAsync(id);
        if (task == null)
        {
            _logger.LogWarning("No task found with id {id}", id);
            throw new ApiException(
                "No task found with id",
                "NotFound",
                404,
                "Task id not found",
                "/api/Task/GetTaskById");
        }
        return task;
    }

    public async Task<TaskItem> CreateTask(TaskItem task)
    { 
        var user = await _db.Users.FindAsync(task.UserId);
        if (user == null)
        {
            _logger.LogWarning("No user found with id {userId}", task.UserId);
            throw new ApiException(
                "User does not exist",
                "NotFound",
                404,
                $"User with id {task.UserId} not found",
                "/api/Task/CreateTask"
            );
        }
        if (string.IsNullOrEmpty(task.Title))
        {
            _logger.LogWarning("Title is null or empty");
            throw new ApiException(
                "Title is null or empty",
                "Conflict",
                409,
                "Title conflict",
                "/api/User/CreateTask"
            );
        }

        if (string.IsNullOrEmpty(task.Description))
        {
            _logger.LogWarning("Description is null or empty");
            throw new ApiException(
                "Description is null or empty",
                "Conflict",
                409,
                "Description conflict",
                "/api/User/CreateTask"
            );
        }
        
        if(task.CreatedAt == default)
            task.CreatedAt = DateTime.UtcNow;
        
        await _db.TaskItems.AddAsync(task);
        await _db.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateTask(int id, TaskItem task)
    {
        var updateTask = await _db.TaskItems.FindAsync(id);
        if (updateTask == null)
        {
            _logger.LogError("No task found with id {id}", id);
            throw new ApiException(
                $"No task found with id {id}",
                "NotFound",
                404,
                "Task id not found",
                "/api/Task/UpdateTask"
            );
        }
        
        if(!string.IsNullOrEmpty(task.Title))
            updateTask.Title = task.Title;
        if(!string.IsNullOrEmpty(task.Description))
            updateTask.Description = task.Description;
        
        updateTask.IsCompleted = task.IsCompleted;
        
        await _db.SaveChangesAsync();
        return updateTask;
    }

    public async Task<bool> DeleteTask(int id)
    {
        if (id <= 0)
        {
            _logger.LogWarning("No task found with id {id}", id);
            throw new ApiException(
                $"Task with id {id} not found",
                "NotFound",
                404,
                "Task id not found",
                "/api/Task/DeleteTask"
            );
        }
        var delerteTas = await _db.TaskItems.FindAsync(id);
        if (delerteTas == null)
        {
            _logger.LogError("No task found with id {id}", id);
            return false;
        }
        _db.TaskItems.Remove(delerteTas);
         await _db.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId)
    {
      
        var user = await _db.Users.FindAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("No user found with id {UserId}", userId);
            throw new ApiException(
                "User does not exist",
                "NotFound",
                404,
                $"User with id {userId} not found",
                "/api/Task/GetTasksByUserId"
            );
        }
        var tasks = await _db.TaskItems
            .Where(t => t.UserId == userId)
            .ToListAsync();

        _logger.LogInformation("Retrieved {Count} tasks for user {UserId}", tasks.Count, userId);

        return tasks;
    }
}
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services;

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
            throw new KeyNotFoundException($"No task found with id {id}");
        }
        return task;
    }

    public async Task<TaskItem> CreateTask(TaskItem task)
    { 
        var user = await _db.Users.FindAsync(task.UserId);
        if (user == null)
        {
            _logger.LogWarning("No user found with id {userId}", task.UserId);
            throw new KeyNotFoundException($"User with id {task.UserId} does not exist");
        }
        if (string.IsNullOrEmpty(task.Title))
        {
            _logger.LogWarning("Title is null or empty");
            throw new ArgumentException("Title is empty", nameof(task.Title));
        }

        if (string.IsNullOrEmpty(task.Description))
        {
            _logger.LogWarning("Description is null or empty");
            throw new ArgumentException("Description is empty", nameof(task.Description));
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
            throw new KeyNotFoundException("No task found with id {id}");
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
        var getTasksByUserId = await _db.TaskItems.
            Where(i => i.UserId == userId)
            .ToListAsync();
        
        _logger.LogInformation("Retrieved tasks for user {UserId}", userId);
        
        return getTasksByUserId;
        
    }
}
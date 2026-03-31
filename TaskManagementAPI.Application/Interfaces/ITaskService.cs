using TaskManagementAPI.Domain.Entities;

namespace TaskManagementAPI.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetAllTasks();
    Task<TaskItem> GetTaskById(int id);
    Task<TaskItem> CreateTask(TaskItem task);
    Task<TaskItem> UpdateTask (int id,TaskItem task);
    Task<bool> DeleteTask(int id);
    Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId);
}
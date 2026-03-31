using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Infrastructure.Data;

namespace TaskManagementAPI.Tests;

public class TaskServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<TaskService>> _mockLogger;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        var option = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(option);
        _mockLogger = new Mock<ILogger<TaskService>>();
        _taskService = new TaskService(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task GetTaskById_ShouldReturnTask_WhenTaskNotFound()
    {
        var nonExistingTaskId = 999;
        
        await Assert.ThrowsAnyAsync<ApiException>(() => _taskService.GetTaskById(nonExistingTaskId));
    }
    
    [Fact]
    public async Task CreateTask_ShouldCreateTask_WhenDataIsValid()
    {
        var user = new User
        {
            Username = "TestUser",
            Password = "111111",
            Email = "Test@mail",
            Id = 1,
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        var newTaskItem = new TaskItem
        {
            UserId = 1,
            Title = "Test Task",
            Description = "Test Task",
            IsCompleted = true,
            
        };
      
        var result = await _taskService.CreateTask(newTaskItem);
        
        Assert.NotNull(result);
       
        var taskInDb = await _context.TaskItems.FindAsync(result.Id);
        Assert.NotNull(taskInDb);
        
    }

    [Fact]
    public async Task CreateTask_ShouldThrowException_WhenUserNotFound()
    {
        var taskWithNonExistentUser = new TaskItem
        {
            Title = "Test Task",
            UserId = 999
        };
        
     var exception = await Assert.ThrowsAnyAsync<ApiException>(() =>
         _taskService.CreateTask(taskWithNonExistentUser));
     
     Assert.Equal(404,exception.StatusCode);

    }

    [Fact]
    public async Task CreateTask_ShouldThrowException_WhenTaskTitleIsEmpty()
    {
        var newUser = new User {Id = 1, Username = "TestUser", Email = "Test@mail" ,Password = "111111"};
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        
        var taskWithNonExistentUser = new TaskItem
        {
            UserId = 1,
            IsCompleted  = true,
            CreatedAt =  DateTime.UtcNow,
            Title = "",
            Description = "Test Task",
        };
        
       var exception = await Assert.ThrowsAnyAsync<ApiException>(() =>
            _taskService.CreateTask(taskWithNonExistentUser));
       
       Assert.Equal(409, exception.StatusCode);
    }

    [Fact]
    public async Task CreateTask_ShouldThrowException_WhenDescriptionIsEmpty()
    {
        var newUser = new User {Id = 1, Username = "TestUser", Email = "Test@mail", Password = "111111"};
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        
        var taskWithNonExistentUser = new TaskItem
        { 
            Title = "Test Task",
            UserId = 1,
            IsCompleted  = true,
            CreatedAt =  DateTime.UtcNow,
            Description = ""
        };
     var exception = await Assert.ThrowsAnyAsync<ApiException>(() => 
            _taskService.CreateTask(taskWithNonExistentUser)); 
     Assert.Equal(409, exception.StatusCode);
    }

    [Fact]
    public async Task UpdateTask_ShouldUpdateTask_WhenDataIsValid()
    {
        var newUser = new User {Id = 1, Username = "TestUser", Email = "Test@mail", Password = "111111"};
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var newTaskItem = new TaskItem
        {
            Id = 1,
            UserId = 1,
            Title = "Old Title",
            Description = "Old Description",
            IsCompleted = true,
        };
        _context.TaskItems.Add(newTaskItem);
        await _context.SaveChangesAsync();

        var updateTaskItem = new TaskItem
        {
            Id = 1,
            UserId = 1,
            Title = "New Title",
            Description = "New Description",
            IsCompleted = false,
        };
        
        var result = await _taskService.UpdateTask(1, updateTaskItem);
        
        var TaskInDb = await _context.TaskItems.FindAsync(result.Id);
        Assert.Equal("New Title", updateTaskItem.Title);
        
    }

    [Fact]
    public async Task UpdateTask_ShouldThrowException_WhenUserNotFound()
    {

        var newTaskItem = new TaskItem
        {
            Id = 999,
        };
        
        var exception = await Assert.ThrowsAnyAsync<ApiException>(() =>
            _taskService.UpdateTask(999, newTaskItem));
        Assert.Equal(404, exception.StatusCode);

    }

    [Fact]
    public async Task DeleteTask_ShouldDeleteTask_WhenDataIsValid()
    {
        var newItem = new TaskItem
        {
            Id = 1,
            UserId = 1,
            Title = "Test Task",
            Description = "Test Task",
            IsCompleted = true,
        };
        _context.TaskItems.Add(newItem);
        await _context.SaveChangesAsync();

        var result = await _taskService.DeleteTask(1);
        
        
        Assert.True(result);
        var userInDb = await _context.Users.FindAsync(1);
        Assert.Null(userInDb);
    }

    [Fact]
    public async Task DeleteTask_ShouldThrowException_WhenUserNotFound()
    {
        var nonExistentId = 999;
        
        var delete = await _taskService.DeleteTask(nonExistentId);
        
        Assert.False(delete);
    }

    [Fact]
    public async Task DeleteTask_ShouldThrowException_WhenIdIsInvalid()
    {
        var nonExistentId = 0;
        
        var exception = await Assert.ThrowsAnyAsync<ApiException>(() =>
            _taskService.DeleteTask(nonExistentId));
        Assert.Equal(404, exception.StatusCode);
    }

    [Fact]
    public async Task GetTaskByUserId_ShouldReturnTask_WhenDataIsValid()
    {
        var newUser = new User {Id = 1, Username = "TestUser", Email = "Test@mail" , Password = "111111"};
        var taskItem = new TaskItem {Id = 1, UserId = 1, Title = "Test Task" ,  Description = "Test Task" , IsCompleted = true, CreatedAt =  DateTime.UtcNow};
        _context.Users.Add(newUser);
        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

      
        var getByUserId = await _taskService.GetTasksByUserId(newUser.Id);
        
        Assert.NotNull(getByUserId);
    }

    [Fact]
    public async Task GetTaskByUserId_ShouldThrowException_WhenUserNotFound()
    {
        var nonExistentId = 999;

        var exception = await Assert.ThrowsAnyAsync<ApiException>(() =>
            _taskService.GetTasksByUserId(999));
        Assert.Equal(404, exception.StatusCode);
    }
}
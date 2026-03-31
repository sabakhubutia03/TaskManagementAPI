using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Infrastructure.Data;
using TaskManagementAPI.Infrastructure.Service;

namespace TaskManagementAPI.Tests;

public class UserServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly Mock<ILogger<UserService>> _mockLogger;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        var option = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(option);
        
        _mockLogger = new Mock<ILogger<UserService>>();
        
        _userService = new UserService(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task GetUserById_ShouldThrowApiException_WhenUserNotFound()
    {
        var nonExistentId = 999;
        
        await Assert.ThrowsAsync<ApiException>(() => _userService.GetUserById(nonExistentId));
    }

    [Fact]
    public async Task GetUserById_ShouldReturnUser_WhenUserFound()
    {
        var testUser = new User
        {
            Id = 1, 
            Username = "TestUser",
            Email = "test@gmail.com", 
            CreatedAt = DateTime.UtcNow,
            Password = "12314"
        };
        _context.Users.Add(testUser);
        await _context.SaveChangesAsync();
        
        var result = await _userService.GetUserById(1);
        
        Assert.NotNull(result);
        Assert.Equal("TestUser", result.Username);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateUser_ShouldReturnUser_WhenDataIsValid()
    {
        var newUser = new User
        {
            Username = "TestUser",
            Email = "new@test.com",
            Password = "111111"
        };
        
        var result  = await _userService.CreateUser(newUser);
        
        Assert.NotNull(result);
        Assert.Equal("new@test.com", result.Email);
        
        var userInDb = await _context.Users.FirstOrDefaultAsync(u => u.Email == result.Email);
        Assert.NotNull(userInDb);
    }

    [Fact]
    public async Task createUser_ShouldThrowApiException_WhenEmailIsEmpty()
    {
        var invalidUser = new User
        {
            Username = "TestUser",
            Email = "",
            Password = "111111"
        };
        
        var exception = await Assert.ThrowsAsync<ApiException>(() => _userService.CreateUser(invalidUser));
        Assert.Equal(409, exception.StatusCode);
    }

    [Fact]
    public async Task UpdateUser_ShouldThrowApiException_WhenUserNotFound()
    {
        var nonExistentId = 999;

        var newUser = new User
        {
            Id = nonExistentId,
            Username = "TestUser",
            Email = "new@test.com",
            Password = "111111",
            CreatedAt = DateTime.UtcNow,
        };

        await Assert.ThrowsAsync<ApiException>(() => _userService.UpdateUser(nonExistentId, newUser));
    }

    [Fact]
    public async Task DeleteUser_ShouldReturnTrue_WhenUserDeleted()
    {

        var newUser = new User
        {
            Id = 1,
            Username = "TestUser",
            Email = "Test@mail.com",
            Password = "111111",
            CreatedAt = DateTime.UtcNow,
        };
        
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        
        var result = await _userService.DeleteUser(1);
        
        Assert.True(result);
        var userInDb = await _context.Users.FindAsync(1);
        Assert.Null(userInDb);

    }

    [Fact]
    public async Task DeleteUser_ShouldReturnFalse_WhenUserNotFound()
    {
        var nonExistentId = 999;
        
        var result = await _userService.DeleteUser(nonExistentId);
        
        Assert.False(result);
        
    }
}
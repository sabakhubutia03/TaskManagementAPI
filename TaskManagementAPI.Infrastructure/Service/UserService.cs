using TaskManagementAPI.Application.Interfaces;
using TaskManagementAPI.Domain.Entities;
using TaskManagementAPI.Exceptions;
using TaskManagementAPI.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace TaskManagementAPI.Infrastructure.Service;


public class UserService: IUserService
{ 
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
        => await _context.Users.ToListAsync();

    public async Task<User> GetUserById(int id)
    {
        var GetById = await _context.Users.FindAsync(id);
        if (GetById == null)
        {
            _logger.LogError("User with id {id} id not found" , id);
            throw new ApiException(
                "Not found user",
                "NotFound",
                404,
                $"User with id {id} id not found",
                "/api/user/GetUserById");
        }
        return GetById;
    }

    public async Task<User> CreateUser(User user)
    {
        if (string.IsNullOrEmpty(user.Email))
        {
            _logger.LogWarning("Email is null or empty");
            throw new ApiException(
                "Email is empty",
                "Conflict",
                409,
                "Email is null or empty",
                "/api/user/CreateUser"
            );
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            _logger.LogWarning("Password is null or empty");
            throw new ApiException(
                "Password is empty",
                "Conflict",
                409,
                "Password is null or empty",
                "/api/user/CreateUser"
            );
        } 
        if(user.CreatedAt == default)
            user.CreatedAt = DateTime.UtcNow;
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUser(int id, User user)
    {
        var updateUser = await _context.Users.FindAsync(id);
        if (updateUser == null) 
        {
            _logger.LogError("User with id {id} id not found", id);
            throw new ApiException(
                "User id not found",
                "NotFound",
                404,
                $"User with id {id} not found",
                "/api/user/UpdateUser"
            );
        } 
     
        if(!string.IsNullOrEmpty(user.Username))
            updateUser.Username = user.Username;
        
        if(!string.IsNullOrEmpty(user.Email))
            updateUser.Email = user.Email;
        
        await _context.SaveChangesAsync();
        return updateUser;
    }

    public async Task<bool> DeleteUser(int id)
    {
        var deleteUser = await _context.Users.FindAsync(id);
        if (deleteUser == null)
        {
            _logger.LogInformation("User is null or empty");
            return false;
        }
        
     
        _context.Remove(deleteUser);
        await _context.SaveChangesAsync();
        return true;
    }
}


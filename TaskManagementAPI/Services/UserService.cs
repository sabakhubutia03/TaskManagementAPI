using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Data;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services;

public class UserService : IUserService
{ 
    public readonly ApplicationDbContext _context;
    public readonly ILogger<UserService> _logger;

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
            throw new KeyNotFoundException($"User with id {id} not found");
        }
        return GetById;
    }

    public async Task<User> CreateUser(User user)
    {
        if (string.IsNullOrEmpty(user.Email))
        {
            _logger.LogWarning("Email is null or empty");
            throw new ArgumentException("Email is empty", nameof(user.Email));
        }

        if (string.IsNullOrEmpty(user.Password))
        {
            _logger.LogWarning("Password is null or empty");
            throw new Exception("Password is empty");
        } 
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
            throw new Exception("User with id {id} id not found");
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
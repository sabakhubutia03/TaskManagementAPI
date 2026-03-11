using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Domain.Entities;

namespace TaskManagementAPI.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
}
using System.Text.Json.Serialization;

namespace TaskManagementAPI.Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt {get; set;}
    
    [JsonIgnore]
    public ICollection<TaskItem> Tasks { get; set; } =  new List<TaskItem>();
}
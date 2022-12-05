using AspTestStage.BaseClasses;

namespace AspTestStage.Database.Domain;

public class User : EntityBase
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthdate { get; set; }
    public string FullName { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiryTime { get; set; }

    public int RoleId { get; set; }
    public Role? Role { get; set; }

    public List<GroupStudent> GroupStudents { get; set; } = new();
    public List<Group> Groups { get; set; } = new();
}
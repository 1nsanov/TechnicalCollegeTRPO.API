namespace AspTestStage.Dto;

public class UserSingUp
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public DateTime? Birthdate { get; set; }
    public string FullName { get; set; }
    public int RoleId { get; set; }
}
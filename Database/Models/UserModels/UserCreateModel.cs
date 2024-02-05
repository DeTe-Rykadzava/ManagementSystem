namespace Database.Models.UserModels;

public class UserCreateModel
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    public int RoleId { get; set; }

}
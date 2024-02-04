namespace Database.Models.UserModels;

public class UserCreateModel
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;
}
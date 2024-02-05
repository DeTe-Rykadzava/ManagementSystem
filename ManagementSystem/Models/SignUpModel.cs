using System.ComponentModel.DataAnnotations;
using Database.Models.UserModels;

namespace ManagementSystem.Models;

public class SignUpModel
{
    [Required(ErrorMessage = "The login field is required.", AllowEmptyStrings = false)]
    [EmailAddress(ErrorMessage = "The login must be an email address, for example example@mail.com")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "The password field is required.", AllowEmptyStrings = false)]
    public string Password { get; set; } = null!;
    
    [Required(ErrorMessage = "The password field is required.", AllowEmptyStrings = false)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "The password field is required.", AllowEmptyStrings = false)]
    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    [Required(ErrorMessage = "Role id is required field.", AllowEmptyStrings = false)]
    [Range(1, 4, ErrorMessage = "Unknown role id.")]
    public int RoleId { get; set; } = 1;
    
    public UserCreateModel ConvertToDatabaseModel()
    {
        var model = new UserCreateModel
        {
            Login = this.Login,
            Password = this.Password,
            Patronymic = this.Patronymic,
            FirstName = this.FirstName,
            LastName = this.LastName,
            RoleId = this.RoleId
        };
        return model;
    }
}
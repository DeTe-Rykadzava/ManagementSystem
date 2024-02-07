using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.AuthModels;

public class SignInModel
{
    [Required(ErrorMessage = "Login is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "SignIn is email")]
    [EmailAddress(ErrorMessage = "The login must be an email address, for example example@mail.com")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Password is required", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
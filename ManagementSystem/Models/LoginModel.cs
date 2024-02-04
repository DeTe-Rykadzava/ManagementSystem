using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models;

public class LoginModel
{
    [Required(ErrorMessage = "SignIn is required")]
    [DataType(DataType.EmailAddress, ErrorMessage = "SignIn is email")]
    [EmailAddress]
    public string Login { get; set; }

    [Required( ErrorMessage = "Password is required", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models;

public class LoginModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    public string Login { get; set; } = null!;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
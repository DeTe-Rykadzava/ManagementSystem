using System.ComponentModel.DataAnnotations;

namespace ManagementSystem.Models;

public class LoginModel
{
    [Required]
    public string Login { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
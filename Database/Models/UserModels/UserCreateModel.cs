using System.ComponentModel.DataAnnotations;

namespace Database.Models.UserModels;

public class UserCreateModel
{
    [Required]
    [DataType(DataType.EmailAddress)]

    public string Login { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;
    
    [Required]
    public string FirstName { get; set; } = null!;
    
    [Required]
    public string LastName { get; set; } = null!;

    public string? Patronymic { get; set; }

    [Required]
    public int RoleId { get; set; }

}
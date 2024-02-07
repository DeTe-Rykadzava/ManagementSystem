namespace ManagementSystem.AuthModels;

public class AuthResultModel
{
    public string Message { get; set; } = null!;
    public string? Token { get; set; } 
    public bool IsSuccess { get; set; }
}
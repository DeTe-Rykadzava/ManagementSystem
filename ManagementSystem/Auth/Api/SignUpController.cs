using ManagementSystem.Models;
using ManagementSystem.Service;
using Microsoft.AspNetCore.Mvc;

namespace ManagementSystem.Auth.Api;

[ApiController]
public class SignUpController : ControllerBase
{
    private readonly ILogger<SignInController> _logger;

    private readonly UserService _userService;
    
    public SignUpController(ILogger<SignInController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpPost]
    [Route("/api/sign_up")]
    public async Task<AuthResultModel> SignUp()
    {
        return new AuthResultModel{ IsSuccess = true, Message = "Success"};
    }
}
using System.Security.Claims;
using System.Text.Json;
using ManagementSystem.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ManagementSystem.Auth;

public class AuthProvider : AuthenticationStateProvider
{
    private readonly ILogger<AuthProvider> _logger;
    
    private readonly HttpContext? _context;

    public readonly HttpClient _client;

    public AuthProvider(ILogger<AuthProvider> logger, IHttpContextAccessor contextAccessor, HttpClient client)
    {
        _logger = logger;
        _context = contextAccessor.HttpContext;
        _client = client;
    }
    
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            if(_context == null)
                return new AuthenticationState(new ClaimsPrincipal());
            
            if (_context.User.Identity?.IsAuthenticated == null || _context.User.Identity.IsAuthenticated == false)
            {
                return new AuthenticationState(new ClaimsPrincipal());
            }

            return new AuthenticationState(new ClaimsPrincipal(_context.User.Identity));
        }
        catch (Exception e)
        {
            _logger.LogError("Error while check auth user.\n{Message}\n{InnerException}", e.Message, e.InnerException);
            return new AuthenticationState(new ClaimsPrincipal());
        }
    }

    public async Task<AuthResultModel> LoginAsync(SignInModel model)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(model));
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/json");

            var result = await _client.PostAsync("api/auth/sign_in", content);
            if (!result.IsSuccessStatusCode)
                return new AuthResultModel { Message = "Unauthorized", IsSuccess = false};

            var token = await JsonSerializer.DeserializeAsync<string>(await result.Content.ReadAsStreamAsync());

            return new AuthResultModel { Message = "Success", IsSuccess = true, Token = token};
        }
        catch (Exception e)
        {
            _logger.LogError("Error on SignIn async AuthProvider.\n{Message}\n{InnerException}", e.Message, e.InnerException);
            return new AuthResultModel { Message = "Error while login user", IsSuccess = false};
        }
    }

    public async Task LogOutAsync(NavigationManager manager)
    {
        manager.NavigateTo("api/auth/sign_out");
    }
}
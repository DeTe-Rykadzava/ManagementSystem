using Microsoft.AspNetCore.Components.Authorization;

namespace ManagementSystem.Auth;

public class AuthProvider : AuthenticationStateProvider
{
    private readonly ILogger<AuthProvider> _logger;
    private readonly 
    
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        
    }
}
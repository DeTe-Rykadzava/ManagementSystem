using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ManagementSystem.Models;
using ManagementSystem.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ManagementSystem.Auth.Api;

[ApiController]
public class AuthController : ControllerBase
{
    private class AuthOptions
    {
        public const string ISSUER = "ManagementSystemIdentity";
        public const string AUDIENCE = "ManagementSystemClient";
        private const string KEY = "CWnIZcKzM8doT9nN8SpxO64NsgGEiZHr";

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
    
    private readonly ILogger<AuthController> _logger;

    private readonly UserService _userService;
    
    public AuthController(ILogger<AuthController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }
    
    [HttpPost]
    [Route("api/auth/sign_in")]
    public async Task<ActionResult<string>> LoginAsync(LoginModel model)
    {
        try
        {
            var user = await _userService.GetUserByModel(model);
            if (user == null)
                return Unauthorized();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Login),
                new Claim(ClaimTypes.Name, user.Login),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme); 
            
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                claims: claimsIdentity.Claims,
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return Ok(encodedJwt);
        }
        catch (Exception e)
        {
            _logger.LogError("Error with login user.\n{Message}\n{InnerException}", e.Message, e.InnerException);
#if DEBUG
            return Problem($"Error with login user.\n{e.Message}\n{e.InnerException}");
#endif
            return Problem("Error with login user");
        }
    }
    
    [HttpGet]
    [Route("api/auth/sign_in/{jwtToken}&{redirectUrl}")]
    public async Task<IActionResult> LoginInClientAsync(string jwtToken, string? redirectUrl = null)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);

            if (token.Issuer != AuthOptions.ISSUER ||
                token.Audiences.FirstOrDefault(x => x == AuthOptions.AUDIENCE) == null)
            {
                return BadRequest("[KVAK] token is not valid =(");
            }

            var identity = new ClaimsIdentity(token.Claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties());
            return Redirect(redirectUrl ?? "/");
        }
        catch (Exception e)
        {
            _logger.LogError("Error with login user on client.\n{Message}\n{InnerException}", e.Message, e.InnerException);
#if DEBUG
            return Problem($"Error with login user.\n{e.Message}\n{e.InnerException}");
#endif
            return Problem("Error with login user");
        }
    }

    [HttpGet]
    [Route("api/auth/sign_out")]
    public async Task<IActionResult> LogOutAsync()
    {
        await HttpContext.SignOutAsync();
        return Redirect("/");
    }
}
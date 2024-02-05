using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace ManagementSystem.Auth;

public class AuthOptions
{
    
    public const string ISSUER = "ManagementSystemIdentity";
    public const string AUDIENCE = "ManagementSystemClient";
    private const string KEY = "CWnIZcKzM8doT9nN8SpxO64NsgGEiZHr";

    public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}
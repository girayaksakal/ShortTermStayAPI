using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthController : ControllerBase {
    private readonly JwtSettings _jwtSettings;

    public AuthController(IOptions<JwtSettings> jwtSettings) {
        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] LoginRequest request) {
        var user = InMemoryUsers.Users.SingleOrDefault(u => u.Username == request.Username && u.Password == request.Password);
        if (user == null) {return Unauthorized(new { Message = "Invalid username or password" });}

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddHours(72),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);

        return Ok(new { 
            Token = tokenString,
            Username = user.Username,
            Role = user.Role
        });
    } 
}

public class LoginRequest {
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public static class InMemoryUsers
{
    public static List<User> Users = new()
    {
        new User { Username = "admin", Password = "string", Role = "Admin" },
        new User { Username = "host", Password = "string", Role = "Host" },
        new User { Username = "guest", Password = "string", Role = "Guest" }
    };
}

public class User
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}
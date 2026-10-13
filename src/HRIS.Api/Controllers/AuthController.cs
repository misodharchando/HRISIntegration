using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace HRIS.Api.Controllers;

public sealed record LoginRequest(string Username, string Password);

[ApiController, Route("api/auth")]
[AllowAnonymous]
public sealed class AuthController(IConfiguration c) : ControllerBase
{
    [HttpPost("token")]
    public IActionResult Token(LoginRequest x)
    {
        if (x.Username != c["DemoUser:Username"] || x.Password != c["DemoUser:Password"])
            return Unauthorized();
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, x.Username),
            new Claim(ClaimTypes.Role, "HRISIntegration"),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(c["Jwt:Key"]!));
        var token = new JwtSecurityToken(
            c["Jwt:Issuer"],
            c["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new(key, SecurityAlgorithms.HmacSha256)
        );
        return Ok(
            new { accessToken = new JwtSecurityTokenHandler().WriteToken(token), expiresIn = 3600 }
        );
    }
}

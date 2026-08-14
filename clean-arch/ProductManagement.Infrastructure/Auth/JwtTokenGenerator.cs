
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductManagement.Application.Interfaces;

namespace ProductManagement.Infrastructure.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public string GenerateToken(Guid userId, string email)
    {

        var clains = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userId.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Email, email)
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: clains,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds
        );

        // Implement JWT token generation logic here
        // For example, using System.IdentityModel.Tokens.Jwt and Microsoft.IdentityModel.Tokens
        // This is a placeholder implementation
        return $"Token for user {userId} with email {email} {new JwtSecurityTokenHandler().WriteToken(token)}";
    }
}
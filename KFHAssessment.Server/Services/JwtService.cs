using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using KFHAssessment.Server.Entities;
using Microsoft.IdentityModel.Tokens;

namespace KFHAssessment.Server.Services;

public class JwtService
{
    private readonly IConfiguration _config;
    public JwtService(IConfiguration config) => _config = config;

    public string Generate(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,        user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username),
            new Claim(ClaimTypes.Role,                    user.Role)
        };
        var token = new JwtSecurityToken(
            _config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
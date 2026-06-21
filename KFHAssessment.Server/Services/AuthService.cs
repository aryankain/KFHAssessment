using KFHAssessment.Server.Data;
using KFHAssessment.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace KFHAssessment.Server.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly JwtService _jwt;

    public AuthService(AppDbContext db, JwtService jwt)
    { _db = db; _jwt = jwt; }

    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto dto)
    {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == dto.Username && u.IsActive);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return null;

        return new LoginResponseDto
        {
            Token = _jwt.Generate(user),
            Username = user.Username,
            Role = user.Role,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}
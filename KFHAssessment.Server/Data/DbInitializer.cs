using KFHAssessment.Server.Entities;
using KFHAssessment.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace KFHAssessment.Server.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider sp)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

        if (!await db.Users.AnyAsync())
        {
            db.Users.Add(new User
            {
                Username = "admin",
                Email = "admin@kfh.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@1234"),
                Role = "Admin",
                IsActive = true,
                CreatedDate = DateTime.UtcNow
            });
            await db.SaveChangesAsync();
        }

        if (!await db.Loans.AnyAsync())
        {
            var adminId = (await db.Users.FirstAsync()).Id;
            db.Loans.AddRange(
                new Loan { ApplicantName = "Izhar Anjum", LoanAmount = 25000, CreditScore = 760, Status = LoanStatus.Pending, CreatedByUserId = adminId }
            );
            await db.SaveChangesAsync();
        }
    }
}
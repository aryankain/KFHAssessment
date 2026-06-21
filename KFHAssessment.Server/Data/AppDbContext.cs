using KFHAssessment.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace KFHAssessment.Server.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> o) : base(o) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureUsers(modelBuilder);
        ConfigureLoans(modelBuilder);
        ConfigureAuditLogs(modelBuilder);
    }

    private static void ConfigureUsers(ModelBuilder m)
    {
        var e = m.Entity<User>();
        e.ToTable("Users");
        e.HasKey(u => u.Id);
        e.Property(u => u.Username).IsRequired().HasMaxLength(100);
        e.Property(u => u.Email).IsRequired().HasMaxLength(200);
        e.Property(u => u.PasswordHash).IsRequired().HasMaxLength(500);
        e.Property(u => u.Role).IsRequired().HasMaxLength(50);
        e.HasIndex(u => u.Username).IsUnique();
        e.HasIndex(u => u.Email).IsUnique();
    }

    private static void ConfigureLoans(ModelBuilder m)
    {
        var e = m.Entity<Loan>();
        e.ToTable("Loans");
        e.HasKey(l => l.Id);
        e.Property(l => l.ApplicantName).IsRequired().HasMaxLength(100);
        e.Property(l => l.Status).HasConversion<int>();
        e.HasOne(l => l.CreatedBy)
         .WithMany()
         .HasForeignKey(l => l.CreatedByUserId)
         .OnDelete(DeleteBehavior.Restrict);
        e.HasIndex(l => l.Status);
        e.HasIndex(l => l.CreatedByUserId);
    }

    private static void ConfigureAuditLogs(ModelBuilder m)
    {
        var e = m.Entity<AuditLog>();
        e.ToTable("AuditLogs");
        e.HasKey(a => a.Id);
        e.Property(a => a.Action).IsRequired().HasMaxLength(100);
        e.Property(a => a.Entity).IsRequired().HasMaxLength(100);
        e.Property(a => a.Detail).HasMaxLength(1000);
        e.Property(a => a.IpAddress).HasMaxLength(50);
        e.HasIndex(a => a.UserId);
    }
}
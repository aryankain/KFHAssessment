using System.Security.Claims;
using KFHAssessment.Server.Data;
using KFHAssessment.Server.Entities;
using KFHAssessment.Shared.DTOs;
using KFHAssessment.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace KFHAssessment.Server.Services;

public class LoanService
{
    private readonly AppDbContext _db;
    private readonly LoanEvaluationService _evaluator;
    private readonly IHttpContextAccessor _http;

    public LoanService(
        AppDbContext db,
        LoanEvaluationService evaluator,
        IHttpContextAccessor http)
    {
        _db = db;
        _evaluator = evaluator;
        _http = http;
    }

    private int UserId =>
        int.TryParse(
            _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id) ? id : 0;

    public async Task<List<LoanDto>> GetAllAsync() =>
        await _db.Loans
            .AsNoTracking()
            .OrderByDescending(l => l.CreatedDate)
            .Select(l => ToDto(l))
            .ToListAsync();

    public async Task<LoanDto?> GetByIdAsync(int id) =>
        await _db.Loans
            .AsNoTracking()
            .Where(l => l.Id == id)
            .Select(l => ToDto(l))
            .FirstOrDefaultAsync();

    public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
    {
        var loan = new Loan
        {
            ApplicantName = dto.ApplicantName.Trim(),
            LoanAmount = dto.LoanAmount,
            CreditScore = dto.CreditScore,
            Status = LoanStatus.Pending,
            CreatedDate = DateTime.UtcNow,
            CreatedByUserId = UserId
        };

        _db.Loans.Add(loan);
        await _db.SaveChangesAsync();
        await AuditAsync("LoanCreated", "Loan", loan.Id,
            $"{loan.ApplicantName} — amount {loan.LoanAmount:N2}");

        return ToDto(loan);
    }

    public async Task<LoanEvaluationResultDto?> EvaluateAsync(int id)
    {
        var loan = await _db.Loans.FindAsync(id);
        if (loan is null) return null;

        var decision = _evaluator.Evaluate(loan.LoanAmount, loan.CreditScore);

        loan.Status = decision.Status;
        await _db.SaveChangesAsync();
        await AuditAsync("LoanEvaluated", "Loan", loan.Id, decision.Reason);

        return new LoanEvaluationResultDto
        {
            LoanId = id,
            Status = decision.Status,
            Reason = decision.Reason
        };
    }

    private async Task AuditAsync(string action, string entity,
                                   int? entityId, string? detail)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            UserId = UserId == 0 ? null : UserId,
            Action = action,
            Entity = entity,
            EntityId = entityId,
            Detail = detail,
            OccurredAt = DateTime.UtcNow,
            IpAddress = _http.HttpContext?.Connection
                              .RemoteIpAddress?.ToString()
        });
        await _db.SaveChangesAsync();
    }

    private static LoanDto ToDto(Loan l) => new()
    {
        Id = l.Id,
        ApplicantName = l.ApplicantName,
        LoanAmount = l.LoanAmount,
        CreditScore = l.CreditScore,
        Status = l.Status,
        CreatedDate = l.CreatedDate
    };
}
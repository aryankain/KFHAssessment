using System.ComponentModel.DataAnnotations.Schema;
using KFHAssessment.Shared.Enums;

namespace KFHAssessment.Server.Entities;

public class Loan
{
    public int Id { get; set; }
    public string ApplicantName { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal LoanAmount { get; set; }

    public int CreditScore { get; set; }
    public LoanStatus Status { get; set; } = LoanStatus.Pending;
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public int CreatedByUserId { get; set; }
    public User? CreatedBy { get; set; }
}
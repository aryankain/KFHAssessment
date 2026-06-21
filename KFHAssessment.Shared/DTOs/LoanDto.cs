using KFHAssessment.Shared.Enums;

namespace KFHAssessment.Shared.DTOs;

public class LoanDto
{
    public int Id { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public int CreditScore { get; set; }
    public LoanStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
}
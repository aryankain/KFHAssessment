using KFHAssessment.Shared.Enums;

namespace KFHAssessment.Shared.DTOs;

public class LoanEvaluationResultDto
{
    public int LoanId { get; set; }
    public LoanStatus Status { get; set; }
    public string Reason { get; set; } = string.Empty;
}
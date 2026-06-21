using System.ComponentModel.DataAnnotations;

namespace KFHAssessment.Shared.DTOs;

public class CreateLoanDto
{
    [Required(ErrorMessage = "Applicant name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be 2–100 characters.")]
    public string ApplicantName { get; set; } = string.Empty;

    [Range(1, 10_000_000, ErrorMessage = "Loan amount must be between 1 and 10,000,000.")]
    public decimal LoanAmount { get; set; }

    [Range(1, 850, ErrorMessage = "Credit score must be between 1 and 850.")]
    public int CreditScore { get; set; }
}
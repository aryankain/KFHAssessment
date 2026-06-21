namespace KFHAssessment.Server.Services;

public class LoanEvaluationOptions
{
    public const string SectionName = "LoanEvaluation";

    public int HighCreditScore { get; set; } = 700;

    public int MidCreditScore { get; set; } = 650;

    public decimal HighScoreMaxAmount { get; set; } = 50_000m;

    public decimal MidScoreMaxAmount { get; set; } = 20_000m;
}
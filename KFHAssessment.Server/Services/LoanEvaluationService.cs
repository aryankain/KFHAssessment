using KFHAssessment.Shared.Enums;
using Microsoft.Extensions.Options;

namespace KFHAssessment.Server.Services;

public class LoanEvaluationService
{
    private readonly LoanEvaluationOptions _rules;

    public LoanEvaluationService(IOptions<LoanEvaluationOptions> options)
        => _rules = options.Value;

    public LoanDecision Evaluate(decimal loanAmount, int creditScore)
    {
        //good score case
        if (creditScore >= _rules.HighCreditScore
            && loanAmount <= _rules.HighScoreMaxAmount)
        {
            return new LoanDecision(
                LoanStatus.Approved,
                $"Approved: credit score {creditScore} meets high limit " +
                $"(>= {_rules.HighCreditScore}) and amount {loanAmount:N2} " +
                $"is within the {_rules.HighScoreMaxAmount:N2} limit.");
        }

        //mid-level case
        if (creditScore >= _rules.MidCreditScore
            && creditScore < _rules.HighCreditScore
            && loanAmount <= _rules.MidScoreMaxAmount)
        {
            return new LoanDecision(
                LoanStatus.Approved,
                $"Approved (mid-level): credit score {creditScore} in " +
                $"[{_rules.MidCreditScore}, {_rules.HighCreditScore}) and amount " +
                $"{loanAmount:N2} within the {_rules.MidScoreMaxAmount:N2} limit.");
        }

        //reject all other cases
        return new LoanDecision(
            LoanStatus.Rejected,
            BuildRejectionReason(loanAmount, creditScore));
    }

    private string BuildRejectionReason(decimal amount, int score)
    {
        if (score < _rules.MidCreditScore)
            return $"Rejected: credit score {score} is below the minimum " +
                   $"required score of {_rules.MidCreditScore}.";

        if (score >= _rules.HighCreditScore && amount > _rules.HighScoreMaxAmount)
            return $"Rejected: amount {amount:N2} exceeds the " +
                   $"{_rules.HighScoreMaxAmount:N2} limit.";

        if (score >= _rules.MidCreditScore && score < _rules.HighCreditScore
            && amount > _rules.MidScoreMaxAmount)
            return $"Rejected: amount {amount:N2} exceeds the stricter " +
                   $"{_rules.MidScoreMaxAmount:N2} limit for mid-level applicants.";

        return $"Rejected: credit score {score} and amount {amount:N2} " +
               "did not satisfy any approval rule.";
    }
}
public record LoanDecision(LoanStatus Status, string Reason);
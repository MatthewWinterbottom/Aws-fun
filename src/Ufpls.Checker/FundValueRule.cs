using Ufpls.Domain;

namespace Ufpls.Checker;

public class FundValueRule : IUfplsEligibilityRule
{
    public string Name => "FundValueRule";

    public EligibilityResult Evaluate(UfplsCase ufplsCase)
    {
        var passed = ufplsCase.FundValue >= 1000; // Example minimum
        return new EligibilityResult
        {
            RuleName = Name,
            Passed = passed,
            Reason = passed ? null : "Fund value is too low"
        };
    }
}

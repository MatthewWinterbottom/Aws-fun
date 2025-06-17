using Ufpls.Checker;
using Ufpls.Domain;

public class DeceasedRule : IUfplsEligibilityRule
{
    public string Name => "DeceasedRule";

    public EligibilityResult Evaluate(UfplsCase ufplsCase)
    {
        var passed = !ufplsCase.IsDeceased;

        return new EligibilityResult
        {
            Passed = passed,
            RuleName = Name,
            Reason = passed ? null : "Deceased"
        };
    }
}
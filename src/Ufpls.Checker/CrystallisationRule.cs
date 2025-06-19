using Ufpls.Domain;

namespace Ufpls.Checker;

public class CrystallisationRule : IUfplsEligibilityRule
{
    public string Name => "CrystallisationRule";

    public EligibilityResult Evaluate(UfplsCase ufplsCase)
    {
        var crystallisedCutoff = DateTime.UtcNow.AddYears(-5); // Example rule
        var passed = ufplsCase.DateOfCrystallisation >= crystallisedCutoff;
        return new EligibilityResult
        {
            RuleName = Name,
            Passed = passed,
            Reason = passed ? null : "Date of crystallisation too old"
        };
    }
}

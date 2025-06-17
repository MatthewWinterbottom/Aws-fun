using Ufpls.Domain;

namespace Ufpls.Checker;

public class EligibilityEvaluator
{
    private readonly IEnumerable<IUfplsEligibilityRule> _rules;

    public EligibilityEvaluator(IEnumerable<IUfplsEligibilityRule> rules)
    {
        _rules = rules;
    }

    public (bool IsEligible, List<EligibilityResult> Results) Evaluate(UfplsCase ufplsCase)
    {
        var results = _rules.Select(r => r.Evaluate(ufplsCase)).ToList();
        var isEligible = results.All(r => r.Passed);
        return (isEligible, results);
    }
}

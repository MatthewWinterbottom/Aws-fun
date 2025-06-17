using Ufpls.Domain;

namespace Ufpls.Checker;

public interface IUfplsEligibilityRule
{
    string Name { get; }
    EligibilityResult Evaluate(UfplsCase ufplsCase);
}

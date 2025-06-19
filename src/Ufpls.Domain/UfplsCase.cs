using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ufpls.Domain;

public class UfplsCase
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string CaseId { get; set; } = default!;
    public string MemberId { get; set; } = default!;
    public decimal FundValue { get; set; }
    public bool IsDeceased { get; set; }
    public DateTime DateOfCrystallisation { get; set; }
    public UfplsCaseStatus Status { get; set; } = UfplsCaseStatus.Pending;
    public List<EligibilityResult> EligibilityChecks { get; set; } = new();
}

public enum UfplsCaseStatus
{
    Pending,
    Eligible,
    Ineligible
}

public class EligibilityResult
{
    public string RuleName { get; set; } = default!;
    public bool Passed { get; set; }
    public string? Reason { get; set; }
}
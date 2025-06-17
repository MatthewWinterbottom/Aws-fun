using Microsoft.AspNetCore.Mvc;
using Ufpls.Checker;
using Ufpls.Domain;

namespace Ufpls.Api.Controllers;

[ApiController]
[Route("cases")]
public class CasesController : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult GetCase(string id)
    {
        return Ok(new { Id = id, Status = "Pending" });
    }

    [HttpPost("evaluate")]
    public IActionResult EvaluateCase([FromBody] UfplsCase ufplsCase, [FromServices] EligibilityEvaluator evaluator)
    {
        var (isEligible, results) = evaluator.Evaluate(ufplsCase);
        ufplsCase.Status = isEligible ? UfplsCaseStatus.Eligible : UfplsCaseStatus.Ineligible;
        ufplsCase.EligibilityChecks = results;
        return Ok(ufplsCase);
    }
}

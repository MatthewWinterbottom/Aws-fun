using Microsoft.AspNetCore.Mvc;
using Ufpls.Api.Services;
using Ufpls.Checker;
using Ufpls.Domain;

namespace Ufpls.Api.Controllers;

[ApiController]
[Route("cases")]
public class CasesController : ControllerBase
{
    private readonly UfplsCaseService _service;

    public CasesController(UfplsCaseService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCase(string id)
    {
        var result = await _service.GetCaseByIdAsync(id);
        return result is null ? NotFound() : Ok(result);
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

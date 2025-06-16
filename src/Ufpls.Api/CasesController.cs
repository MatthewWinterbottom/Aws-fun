using Microsoft.AspNetCore.Mvc;

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
}

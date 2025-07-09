using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Tomou.Api.Controllers.Medications;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicationsController : ControllerBase
{
    [HttpPost]

    public async Task<IActionResult> Register()
    {
        return Ok();
    }
}

using Microsoft.AspNetCore.Mvc;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.MedicationLog.Get;

namespace Tomou.Api.Controllers.MedicationLog;
[Route("api/[controller]")]
[ApiController]
public class MedicationLogController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseMedicationLogShortJson), StatusCodes.Status200OK)]

    public Task<IActionResult> Get()
    {
    }

}

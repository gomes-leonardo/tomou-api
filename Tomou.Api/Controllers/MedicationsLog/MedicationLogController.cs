using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.MedicationsLog.Get;
using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
using Tomou.Communication.Responses.MedicationLog.Get;

namespace Tomou.Api.Controllers.MedicationLog;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicationLogController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseMedicationsLogJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromServices] IGetMedicationsLogUseCase useCase,
        [FromQuery] MedicationLogQuery query)
    {
        var response = await useCase.Execute(query);
        return Ok(response);
    }
}

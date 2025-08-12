using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.MedicationsLog.Get;
using Tomou.Application.UseCases.MedicationsLog.GetById;
using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Medications.Get;

namespace Tomou.Api.Controllers.MedicationLog;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicationLogController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseMedicationShortJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromServices] IGetMedicationsLogUseCase useCase,
        [FromQuery] MedicationLogQuery query)
    {
        var response = await useCase.Execute(query);
        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseMedicationShortJson), StatusCodes.Status200OK)]
    [Route("{medicamentId}")]

    public async Task<IActionResult> GetById(
        [FromRoute] Guid medicamentId,
        [FromServices] IGetMedicationLogByIdUseCase useCase,
        [FromQuery] Guid ownerId)
    {
        var response = await useCase.Execute(ownerId,  medicamentId);

        if(response is null)
        {
            return NoContent();
        }

        return Ok(response);
    }

}

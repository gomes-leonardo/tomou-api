using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Medications.Get;
using Tomou.Application.UseCases.Medications.GetById;
using Tomou.Application.UseCases.Medications.Register;
using Tomou.Application.UseCases.Medications.Update;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Communication.Requests.Medications.Update;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Dependent.Update;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Communication.Responses.Medications.Register;
using Tomou.Communication.Responses.Medications.Update;

namespace Tomou.Api.Controllers.Medications;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicationsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseRegisterMedicationJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterMedicationsJson request, [FromServices] IRegisterMedicationUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
    [HttpGet]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseMedicationShortJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromServices] IGetMedicationsUseCase useCase,
        [FromQuery] Guid? id = null,
        [FromQuery] string? name = null,
        [FromQuery] string order = "asc")
    {
        var ascending = order.Equals("asc", StringComparison.OrdinalIgnoreCase);
        var response = await useCase.Execute(id, name, ascending);

        return Ok(response);
    }

    [HttpGet]
    [Route("{medicamentId}")]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseMedicationShortJson), StatusCodes.Status200OK)]

    public async Task <IActionResult> GetById(
        [FromRoute] Guid medicamentId,
        [FromServices] IGetMedicationByIdUseCase useCase,
        [FromQuery] Guid? id = null)
    {

        var response = await useCase.Execute(id, medicamentId);

        if (response is not null)
        {
            return Ok(response);
        }

        return NoContent();
    }

    [HttpPut("{medicamentId}")]
    [ProducesResponseType(typeof(ResponseUpdatedMedicationJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(
          [FromRoute] Guid medicamentId,
          [FromServices] IUpdateMedicationUseCase useCase,
          [FromQuery] Guid? id,
          [FromBody] RequestUpdateMedicationJson request)
    {
       
        var response = await useCase.Execute(id, medicamentId, request);
        return Ok(response);
    }

}

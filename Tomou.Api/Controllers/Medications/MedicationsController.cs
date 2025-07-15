using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Medications.Get;
using Tomou.Application.UseCases.Medications.Register;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Medications.Get;
using Tomou.Communication.Responses.Medications.Register;

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
    [ProducesResponseType(typeof(ResponseMedicationsJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromServices] IGetMedicationsUseCase useCase,
        [FromQuery] long? userOrDependentId = null,
        [FromQuery] string? name = null,
        [FromQuery] string order = "asc")
    {
        var ascending = order.Equals("asc", StringComparison.OrdinalIgnoreCase);
        var response = await useCase.Execute(userOrDependentId, name, ascending);

        return Ok(response);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Medications.Register;
using Tomou.Communication.Requests.Medications.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Medications;

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
}

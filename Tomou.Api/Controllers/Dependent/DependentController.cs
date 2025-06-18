using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.User.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Dependent.Register;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Tomou.Api.Controllers.Dependent;
[Route("api/[controller]")]
[ApiController]
public class DependentController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseCreateDependentJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterDependentJson request, [FromServices] IRegisterDependentUseCase useCase)
    {

        var caregiverIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(caregiverIdClaim) || !long.TryParse(caregiverIdClaim, out var caregiverId))
        {
            return Unauthorized(new ResponseErrorJson("Invalid or missing caregiver identifier."));
        }
        var result = await useCase.Execute(caregiverId, request);
        return Created(string.Empty, result);
    }
}

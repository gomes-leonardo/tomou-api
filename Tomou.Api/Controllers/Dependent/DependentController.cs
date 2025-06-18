using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.User.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Dependent.Register;
using Microsoft.AspNetCore.Authorization;

namespace Tomou.Api.Controllers.Dependent;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DependentController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseCreateDependentJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterDependentJson request, [FromServices] IRegisterDependentUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
}

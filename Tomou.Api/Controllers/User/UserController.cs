using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.User.Register;
using Tomou.Communication.Requests.User.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.User.Register;

namespace Tomou.Api.Controllers.User;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost("register")]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request, [FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }



}



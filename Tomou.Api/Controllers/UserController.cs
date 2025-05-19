using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Tomou.Application.UseCases.User.ForgotPassword;
using Tomou.Application.UseCases.User.Register;
using Tomou.Communication.Requests.User.ForgotPassword;
using Tomou.Communication.Requests.User.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.User.Register;

namespace Tomou.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register([FromBody] RequestRegisterUserJson request, [FromServices] IRegisterUserUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }
    

    [HttpPost("forgot-password")]

    public async Task<IActionResult> ForgotPassword(
        [FromBody] RequestForgotPasswordJson request,
        [FromServices] IForgotPasswordUseCase useCase)
    {
        await useCase.Execute(request);
        return NoContent();
    }
}



using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.User.ForgotPassword;
using Tomou.Communication.Requests.User.ForgotPassword;

namespace Tomou.Api.Controllers;
[ApiController]
public class ForgotPasswordController : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [HttpPost("forgot-password")]

    public async Task<IActionResult> ForgotPassword(
        [FromBody] RequestForgotPasswordJson request,
        [FromServices] IForgotPasswordUseCase useCase)
    {
        await useCase.Execute(request);
        return NoContent();
    }
}

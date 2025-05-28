using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.User.ResetPassword;
using Tomou.Communication.Requests.User.ForgotPassword;

namespace Tomou.Api.Controllers;
[ApiController]
public class ResetPasswordController : ControllerBase
{
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] RequestResetPasswordJson request, [FromServices] IResetPasswordUseCase useCase)
    {
        await useCase.Execute(request);
        return Created();
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.User.ForgotPassword;
using Tomou.Communication.Requests.User.ForgotPassword;

namespace Tomou.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ForgotPasswordController : ControllerBase
{
    [HttpPost]

    public async Task<IActionResult> ForgotPassword(
        [FromBody] RequestForgotPasswordJson request,
        [FromServices] IForgotPasswordUseCase useCase)
    {
        await useCase.Execute(request);
        return NoContent();
    }
}

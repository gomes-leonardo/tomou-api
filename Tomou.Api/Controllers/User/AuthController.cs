using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.User.Login;
using Tomou.Communication.Requests.User.Login;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.User.Login;

namespace Tomou.Api.Controllers.User;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseLoggedUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] RequestLoginUserJson request, [FromServices] IDoLoginUseCase useCase)
    {
        var result = await useCase.Execute(request);
        return Ok(result);
    }
}

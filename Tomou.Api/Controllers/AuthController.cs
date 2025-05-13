using Microsoft.AspNetCore.Mvc;
using Tomou.Communication.Requests.User.Login;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.User.Login;

namespace Tomou.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseLoggedUserJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login(RequestLoginUserJson request)
    {

        return Ok();
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Dependent.Delete;
using Tomou.Application.UseCases.Dependent.GetAll;
using Tomou.Application.UseCases.Dependent.GetDependentById;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Application.UseCases.Dependent.Update;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Communication.Responses.Dependent.Register;
using Tomou.Communication.Responses.Dependent.Update;

namespace Tomou.Api.Controllers.Dependent;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DependentController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseCreateDependentJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Register(
        [FromBody] RequestRegisterDependentJson request, 
        [FromServices] IRegisterDependentUseCase useCase 
       )
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseDependentsJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDependents(
       [FromServices] IGetDependentsUseCase useCase,
       [FromQuery] string? name = null,
       [FromQuery] string order = "asc"
      )
    {
        var response = await useCase.Execute(nameFilter: name, ascending: order.Equals("asc", StringComparison.OrdinalIgnoreCase));
        if (response.Dependents.Count != 0)
        {
            return Ok(response);
        }
        return NoContent();
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseDependentShortJson), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public async Task<IActionResult> GetDependentById(
        [FromRoute]Guid id,
        [FromServices] IGetDependentByIdUseCase useCase)
    {
        var response = await useCase.Execute(id);
        
        if(response is not null)
        {
            return Ok(response);
        }

        return NoContent();
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseUpdatedDependentJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> Update(
        [FromBody] RequestUpdateDependentJson request,
        [FromServices] IUpdateDependentUseCase useCase,
        [FromRoute] Guid id)
    {
       var result = await useCase.Execute(request, id);

        return Ok(result);
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]

    public async Task<IActionResult> Delete(
        [FromServices] IDeleteDependentUseCase useCase,
        [FromRoute] Guid id)
    {
        await useCase.Execute(id);
        return NoContent();
    }

}

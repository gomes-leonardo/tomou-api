﻿using Microsoft.AspNetCore.Mvc;
using Tomou.Application.UseCases.Dependent.Register;
using Tomou.Communication.Requests.Dependent.Register;
using Tomou.Communication.Responses.User.Register;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.Dependent.Register;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Tomou.Application.Services.Auth;
using Microsoft.AspNetCore.Http.HttpResults;
using Tomou.Application.UseCases.Dependent.GetAll;
using Tomou.Communication.Responses.Dependent.Get;
using Tomou.Communication.Responses.Dependent.Update;
using Tomou.Application.UseCases.Dependent.Update;
using Tomou.Application.UseCases.Dependent.Delete;

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
    public async Task<IActionResult> GetByCaregiverId(
       [FromServices] IGetByCaregiverIdUseCase useCase,
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

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseUpdatedDependentJson), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    
    public async Task<IActionResult> Update(
        [FromBody] RequestUpdateDependentJson request,
        [FromServices] IUpdateDependentUseCase useCase,
        [FromRoute] long id)
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
        [FromRoute] long id)
    {
        await useCase.Execute(id);
        return NoContent();
    }

}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;
using Tomou.Communication.Responses;
using Tomou.Exception;
using Tomou.Exception.ExceptionsBase;

namespace Tomou.Api.Filter;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is TomouException)
        {
            HandleProjectException(context);
        }

        else
        {
            HandleProjectUnknowError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {

        if(context.Exception is TomouException tomouException)
        {
            var errorResponse = new ResponseErrorJson(tomouException.GetErrors());
            context.HttpContext.Response.StatusCode = tomouException.StatusCode;
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }

    private void HandleProjectUnknowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR);

        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomou.Application.Services.Auth;
using Tomou.Application.UseCases.MedicationsLog.Get;
using Tomou.Communication.Requests.MedicationsLog.MedicationLogQuery;
using Tomou.Communication.Responses;
using Tomou.Communication.Responses.MedicationLog.Get;
using Tomou.Domain.Repositories.MedicationLog.Filters;

namespace Tomou.Api.Controllers.MedicationLog;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class MedicationLogController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ResponseMedicationsLogJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get(
        [FromServices] IGetMedicationsLogUseCase useCase,
        [FromServices] IUserContext userContext,
        [FromQuery] MedicationLogQuery query)
    {
        var userId = userContext.GetUserId();
        var isCaregiver = userContext.IsCaregiver();
        
        Guid ownerId;
        if (isCaregiver)
        {
            if (query.Id == null)
            {
                throw new NotFoundException(ResourceErrorMessages.NOT_FOUND_DEPENDENT);
            }
            
            ownerId = query.Id.Value;
        }
        else
        {
            ownerId = userId;
        }

        var filter = new MedicationLogFilter(
            ownerId: ownerId,
            isCaregiver: isCaregiver,
            medicationId: query.MedicationId,
            status: query.Status,
            scheduledFrom: query.ScheduledFrom,
            scheduledTo: query.ScheduledTo,
            takenFrom: query.TakenFrom,
            takenTo: query.TakenTo,
            onlyOverdue: query.OnlyOverdue,
            isSnoozed: query.IsSnoozed,
            nameContains: query.NameContains,
            ascending: query.Order.Equals("asc", StringComparison.OrdinalIgnoreCase),
            page: query.Page,
            pageSize: query.PageSize
        );

        var response = await useCase.Execute(filter);
        return Ok(response);
    }
}

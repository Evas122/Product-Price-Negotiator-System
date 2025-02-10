using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Domain.Commands.MakeOffer;
using PriceNegotiator.Domain.Commands.ProcessOffer;
using PriceNegotiator.Domain.Common.Constants;
using PriceNegotiator.Domain.Dtos.Assortment;
using PriceNegotiator.Domain.Dtos.Paged;
using PriceNegotiator.Domain.Enums;
using Swashbuckle.AspNetCore.Annotations;

namespace PriceNegotiator.Api.Controllers;

public class NegotiationsController : BaseController
{
    private readonly IMediator _mediator;

    public NegotiationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("make-offer")]
    [SwaggerOperation(Summary = "Create offer for product as client and start negotiation process.")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MakeOffer(MakeOfferCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize(Policy = AuthorizationPolicies.EmployeePolicy)]
    [HttpPost("{id}/process-offer")]
    [SwaggerOperation(Summary = "Accept or reject offer as employee.")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ProcessOffer([FromRoute] Guid id, [FromQuery] EmployeeAction action)
    {
        var command = new ProcessOfferCommand(id, action);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
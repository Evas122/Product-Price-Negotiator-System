using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Application.Commands.MakeOffer;
using PriceNegotiator.Application.Commands.ProcessOffer;
using PriceNegotiator.Application.Common.Constants;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Api.Controllers;

public class NegotiationsController : BaseController
{
    private readonly IMediator _mediator;

    public NegotiationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("make-offer")]
    public async Task<IActionResult> MakeOffer(MakeOfferCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [Authorize(Policy = AuthorizationPolicies.EmployeePolicy)]
    [HttpPost("{id}/process-offer")]
    public async Task<IActionResult> ProcessOffer([FromRoute] Guid id, [FromQuery] EmployeeAction action)
    {
        var command = new ProcessOfferCommand(id, action);
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
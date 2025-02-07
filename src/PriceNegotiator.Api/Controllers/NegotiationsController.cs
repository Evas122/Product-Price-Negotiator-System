using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Application.Commands.MakeOffer;

namespace PriceNegotiator.Api.Controllers;

public class NegotiationsController : BaseController
{
    private readonly IMediator _mediator;

    public NegotiationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("make-offer")]
    public async Task<IActionResult> MakeOffer(MakeNegotiationCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Application.Commands.Login;
using PriceNegotiator.Application.Commands.Register;

namespace PriceNegotiator.Api.Controllers;

public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-in")]
    public async Task <IActionResult> SignIn(LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);  
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp(RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Domain.Commands.Login;
using PriceNegotiator.Domain.Commands.Register;
using PriceNegotiator.Domain.Dtos.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace PriceNegotiator.Api.Controllers;

public class AuthController : BaseController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("sign-in")]
    [SwaggerOperation(Summary = "Log in as employee.")]
    [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails),StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task <IActionResult> SignIn([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);  
    }

    [HttpPost("sign-up")]
    [SwaggerOperation(Summary = "Register new employee.")]
    [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}
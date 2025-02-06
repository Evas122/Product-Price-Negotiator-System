using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Application.Commands.CreateProduct;
using PriceNegotiator.Application.Common.Constants;
using PriceNegotiator.Application.Queries.GetPagedProducts;
using PriceNegotiator.Application.Queries.GetProduct;

namespace PriceNegotiator.Api.Controllers;
public class ProductsController : BaseController
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Policy = AuthorizationPolicies.EmployeePolicy)]
    [HttpPost("create-product")]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("products")]
    public async Task<IActionResult> GetProductsAsync([FromQuery] GetPagedProductsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductAsync([FromRoute] Guid id)
    {
        var query = new GetProductQuery(id);

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}

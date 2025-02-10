using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PriceNegotiator.Domain.Commands.CreateProduct;
using PriceNegotiator.Domain.Common.Constants;
using PriceNegotiator.Domain.Dtos.Assortment;
using PriceNegotiator.Domain.Dtos.Paged;
using PriceNegotiator.Domain.Queries.GetPagedProducts;
using PriceNegotiator.Domain.Queries.GetProduct;
using Swashbuckle.AspNetCore.Annotations;

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
    [SwaggerOperation(Summary = "Create a new product.")]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("products")]
    [SwaggerOperation(Summary = "Get paged list of products.")]
    [ProducesResponseType(typeof(PagedDto<ProductDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductsAsync([FromQuery] GetPagedProductsQuery query)
    {
        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get single product.")]
    [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProductAsync([FromRoute] Guid id)
    {
        var query = new GetProductQuery(id);

        var result = await _mediator.Send(query);

        return Ok(result);
    }
}

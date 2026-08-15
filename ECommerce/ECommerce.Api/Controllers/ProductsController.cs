using MediatR;
using Microsoft.AspNetCore.Mvc;
using ECommerce.Application.Features.Products.Commands;
using ECommerce.Application.Features.Products.Queries;

namespace ECommerce.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var productId = await mediator.Send(command);
        
        return CreatedAtAction(nameof(GetProducts), new { id = productId }, productId);
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] GetProductsQuery query)
    {
        var products = await mediator.Send(query);
        
        return Ok(products);
    }
}
using MediatR;

namespace ECommerce.Application.Features.Products.Commands;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock
) : IRequest<Guid>;
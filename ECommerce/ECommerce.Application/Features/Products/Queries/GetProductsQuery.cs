using MediatR;

namespace ECommerce.Application.Features.Products.Queries;

public record GetProductsQuery() : IRequest<IEnumerable<ProductDto>>;

public record ProductDto(Guid Id, string Name, decimal Price, int Stock);
using MediatR;
using ECommerce.Application.Contracts.Persistence;

namespace ECommerce.Application.Features.Products.Queries;

public class GetProductsQueryHandler(IProductRepository productRepository) 
    : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);

        var productDtos = products.Select(p => new ProductDto(
            Id: p.Id,
            Name: p.Name,
            Price: p.Price,
            Stock: p.Stock
        ));

        return productDtos;
    }
}
using MediatR;
using ECommerce.Domain.Entities;
using ECommerce.Application.Contracts.Persistence;

namespace ECommerce.Application.Features.Products.Commands;

public class CreateProductCommandHandler(
    IProductRepository productRepository, 
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateProductCommand, Guid>
{
    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            name: request.Name,
            description: request.Description,
            price: request.Price,
            stock: request.Stock
        );

        await productRepository.AddAsync(product, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}
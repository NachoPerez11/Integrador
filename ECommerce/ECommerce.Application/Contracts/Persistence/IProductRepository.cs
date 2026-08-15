using ECommerce.Domain.Entities;

namespace ECommerce.Application.Contracts.Persistence;

public interface IProductRepository
{
    Task AddAsync(Product product, CancellationToken cancellationToken);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);
}
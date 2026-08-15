namespace ECommerce.Application.Features.Products.Commands;

public record CreateProductResponse(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    bool IsActive
    );
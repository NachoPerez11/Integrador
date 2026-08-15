using ECommerce.Domain.Exceptions;
namespace ECommerce.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public bool IsActive { get; private set; }
    
    // Constructor privado para EF Core
    private Product()
    {
        Name = string.Empty;
        Description = string.Empty;
    }

    // Factory method — única forma de crear un producto válido
    public static Product Create(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name)) 
            throw new DomainRuleException("El nombre del producto es obligatorio.");
        if (price <= 0)
            throw new DomainRuleException("El precio debe ser mayor a cero.");
        if (stock < 0)
            throw new DomainRuleException("El stock no puede ser negativo.");
        return new Product
        {
            Name = name.Trim(),
            Description = description?.Trim() ?? string.Empty,
            Price = price,
            Stock = stock,
            IsActive = true
        };
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice <= 0)
            throw new DomainRuleException("El precio debe ser mayor a cero.");

        Price = newPrice;
    }
    public void AddStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainRuleException("La cantidad debe ser mayor a cero.");
        
        Stock += quantity;
    }
    public void RemoveStock(int quantity)
    {   
        if (quantity <= 0)
            throw new DomainRuleException("La cantidad debe ser mayor a cero.");
        if (Stock < quantity)
            throw new DomainRuleException($"Stock insuficiente. Disponible: {Stock}");
        
        Stock -= quantity;
    }
    public void Deactivate() => IsActive = false;
    public void Activate() => IsActive = true;
}
using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; private set; } 

    private Order() 
    { 
        Status = string.Empty;
    }

    public static Order Create(Guid userId, decimal totalAmount)
    {
        if (userId == Guid.Empty)
            throw new DomainRuleException("El usuario es obligatorio para crear la orden.");
        
        if (totalAmount <= 0)
            throw new DomainRuleException("El monto total de la orden debe ser mayor a cero.");

        return new Order
        {
            UserId = userId,
            TotalAmount = totalAmount,
            Status = "Pending"
        };
    }

    public void MarkAsPaid()
    {
        Status = "Paid";
    }

    public void MarkAsPaymentRejected()
    {
        Status = "PaymentRejected";
    }
}
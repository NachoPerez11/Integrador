namespace ECommerce.Domain.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string Status { get; set; } 

    public Order(Guid userId, decimal totalAmount) 
    {
        UserId = userId;
        TotalAmount = totalAmount;
        Status = "Pending";
    }
}
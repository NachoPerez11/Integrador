namespace PaymentService.Domain.Entities;

public class PaymentRecord
{
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public string TransactionId { get; private set; } = string.Empty;

    public PaymentRecord(Guid orderId, decimal amount)
    {
        OrderId = orderId;
        Amount = amount;
    }

    // La regla vive acá, en el dominio
    public void Process()
    {
        if (Amount <= 100000)
        {
            Status = "Approved";
            TransactionId = $"TX-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        }
        else
        {
            Status = "Rejected";
            TransactionId = string.Empty;
        }
    }
}
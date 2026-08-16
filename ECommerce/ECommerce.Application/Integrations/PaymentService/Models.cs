namespace Ecommerce.Application.Integrations.PaymentService.Models;

public class PaymentRequestDto
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}

public class PaymentResponseDto
{
    public string Status { get; set; } = string.Empty; // Ej: "Approved" o "Rejected"
    public string TransactionId { get; set; } = string.Empty;
}
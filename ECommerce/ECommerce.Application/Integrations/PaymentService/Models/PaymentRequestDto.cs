namespace ECommerce.Application.Integrations.PaymentService.Models;

public class PaymentRequestDto
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}
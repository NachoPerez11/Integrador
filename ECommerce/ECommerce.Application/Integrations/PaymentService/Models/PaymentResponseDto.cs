namespace ECommerce.Application.Integrations.PaymentService.Models;

public class PaymentResponseDto
{
    public string Status { get; set; } = string.Empty; 
    public string TransactionId { get; set; } = string.Empty;
}
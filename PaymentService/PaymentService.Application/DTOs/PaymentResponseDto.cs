namespace PaymentService.Application.DTOs;

public class PaymentResponseDto
{
    public string Status { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
}
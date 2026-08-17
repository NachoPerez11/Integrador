using ECommerce.Application.Integrations.PaymentService.Models;

namespace ECommerce.Application.Integrations.PaymentService;

public interface IPaymentClient
{
    Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken cancellationToken);
}
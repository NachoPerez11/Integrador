using Ecommerce.Application.Integrations.PaymentService.Models;

namespace Ecommerce.Application.Integrations.PaymentService;

public interface IPaymentClient
{
    Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken cancellationToken);
}
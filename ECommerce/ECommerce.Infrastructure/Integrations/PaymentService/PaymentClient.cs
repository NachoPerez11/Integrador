using System.Net.Http.Json;
using Ecommerce.Application.Integrations.PaymentService;
using Ecommerce.Application.Integrations.PaymentService.Models;

namespace Ecommerce.Infrastructure.Integrations.PaymentService;

public class PaymentClient : IPaymentClient
{
    private readonly HttpClient _httpClient;

    public PaymentClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            // El endpoint exacto que pide el profesor en la Opción 1
            var response = await _httpClient.PostAsJsonAsync("/api/payments/process", request, cancellationToken);
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentResponseDto>(cancellationToken: cancellationToken);
            
            return result ?? new PaymentResponseDto { Status = "Error", TransactionId = string.Empty };
        }
        catch (HttpRequestException ex)
        {
            // Acá atrapamos si el servicio de pagos está caído o da error 500
            // Podés loguear el error acá si tenés un ILogger
            return new PaymentResponseDto { Status = "ServiceUnavailable", TransactionId = string.Empty };
        }
        catch (TaskCanceledException)
        {
            // Atrapamos si ocurre un Timeout
            return new PaymentResponseDto { Status = "Timeout", TransactionId = string.Empty };
        }
    }
}
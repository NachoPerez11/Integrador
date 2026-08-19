using System.Net.Http.Json;
using ECommerce.Application.Integrations.PaymentService;
using ECommerce.Application.Integrations.PaymentService.Models;

namespace ECommerce.Infrastructure.Integrations.PaymentService;

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
            var response = await _httpClient.PostAsJsonAsync("/api/payments/process", request, cancellationToken);
            
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<PaymentResponseDto>(cancellationToken: cancellationToken);
            
            return result ?? new PaymentResponseDto { Status = "Error", TransactionId = string.Empty };
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"\n---> ERROR DE CONEXIÓN: {ex.Message}\n");
            
            return new PaymentResponseDto { Status = "ServiceUnavailable", TransactionId = string.Empty };
        }
        catch (TaskCanceledException)
        {
            return new PaymentResponseDto { Status = "Timeout", TransactionId = string.Empty };
        }
    }
}
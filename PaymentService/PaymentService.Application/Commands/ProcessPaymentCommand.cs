using MediatR;
using PaymentService.Application.DTOs;

namespace PaymentService.Application.Commands;

// El Command implementa IRequest indicando qué DTO va a devolver[cite: 1]
public class ProcessPaymentCommand : IRequest<PaymentResponseDto>
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}
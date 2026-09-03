using MediatR;
using PaymentService.Application.DTOs;

namespace PaymentService.Application.Commands;

public class ProcessPaymentCommand : IRequest<PaymentResponseDto>
{
    public Guid OrderId { get; set; }
    public decimal Amount { get; set; }
}
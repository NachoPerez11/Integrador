using MediatR;
using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Commands;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentResponseDto>
{
    public Task<PaymentResponseDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new PaymentRecord(request.OrderId, request.Amount);
        
        payment.Process();

        return Task.FromResult(new PaymentResponseDto
        {
            Status = payment.Status,
            TransactionId = payment.TransactionId
        });
    }
}
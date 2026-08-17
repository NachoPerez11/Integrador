using MediatR;
using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Commands;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, PaymentResponseDto>
{
    public Task<PaymentResponseDto> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // 1. Instanciamos la entidad de dominio
        var payment = new PaymentRecord(request.OrderId, request.Amount);
        
        // 2. Ejecutamos la regla de negocio[cite: 1]
        payment.Process();

        // (Nota: Si tuvieras EF Core configurado, acá iría el _repository.SaveAsync(payment))

        // 3. Retornamos el DTO de respuesta[cite: 1]
        return Task.FromResult(new PaymentResponseDto
        {
            Status = payment.Status,
            TransactionId = payment.TransactionId
        });
    }
}
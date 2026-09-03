using MediatR;
using ECommerce.Application.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Application.Integrations.PaymentService; 
using ECommerce.Application.Integrations.PaymentService.Models; 

namespace ECommerce.Application.Features.Orders.Commands;

public class CreateOrderCommandHandler(
    IOrderRepository orderRepository,
    IPaymentClient paymentClient,
    IUnitOfWork unitOfWork) 
    : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create(request.UserId, request.TotalAmount);
        
        var paymentRequest = new PaymentRequestDto 
        { 
            OrderId = order.Id, 
            Amount = order.TotalAmount 
        };

        var paymentResponse = await paymentClient.ProcessPaymentAsync(paymentRequest, cancellationToken);
        Console.WriteLine($"\n---> ESTADO DEVUELTO POR PAYMENT SERVICE: {paymentResponse.Status}\n");

        if (paymentResponse.Status == "Approved")
        {
            order.MarkAsPaid(); 
        }
        else
        {
            order.MarkAsPaymentRejected();
        }

        await orderRepository.AddAsync(order, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return order.Id;
    }
}
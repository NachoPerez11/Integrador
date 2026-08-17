using MediatR;
using ECommerce.Application.Contracts.Persistence;
using ECommerce.Domain.Entities;
using ECommerce.Application.Integrations.PaymentService; // Para IPaymentClient
using ECommerce.Application.Integrations.PaymentService.Models; // Para los DTOs

namespace ECommerce.Application.Features.Orders.Commands;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentClient _paymentClient; // <-- Agregamos la interfaz del cliente HTTP

    // Inyectamos ambas dependencias en el constructor
    public CreateOrderCommandHandler(IOrderRepository orderRepository, IPaymentClient paymentClient)
    {
        _orderRepository = orderRepository;
        _paymentClient = paymentClient;
    }

    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. Instanciamos la nueva orden
        var order = new Order(request.UserId, request.TotalAmount);
        
        // 2. Armamos el DTO para el servicio de pagos con los datos de la orden
        var paymentRequest = new PaymentRequestDto 
        { 
            OrderId = order.Id, 
            Amount = order.TotalAmount 
        };

        // 3. Llamamos al microservicio (PaymentService) para procesar el pago
        var paymentResponse = await _paymentClient.ProcessPaymentAsync(paymentRequest, cancellationToken);
        Console.WriteLine($"\n---> ESTADO DEVUELTO POR PAYMENT SERVICE: {paymentResponse.Status}\n");


        // 4. Cambiamos el estado de la orden según lo que responda el servicio[cite: 1]
        if (paymentResponse.Status == "Approved")
        {
            order.Status = "Paid"; 
        }
        else
        {
            order.Status = "PaymentRejected";
        }

        // 5. Guardamos la orden con su estado definitivo en la base de datos
        await _orderRepository.AddAsync(order, cancellationToken);
        
        return order.Id;
    }
}
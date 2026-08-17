using MediatR;
using Microsoft.AspNetCore.Mvc;
using PaymentService.Application.Commands;

namespace PaymentService.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("process")]
    public async Task<IActionResult> ProcessPayment([FromBody] ProcessPaymentCommand command)
    {
        // El endpoint recibe { orderId, amount } a través del Command[cite: 1]
        var result = await _mediator.Send(command);
        
        // Devuelve { status, transactionId } a través del DTO[cite: 1]
        return Ok(result); 
    }
}
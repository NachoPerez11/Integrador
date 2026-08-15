using MediatR;

namespace ECommerce.Application.Features.Auth.Commands;

public record RegisterUserCommand(
    string Name,
    string Email,
    string Password,
    string Role
) : IRequest<Guid>;
using MediatR;
using ECommerce.Application.Contracts.Persistence;
using ECommerce.Application.Contracts.Security;

namespace ECommerce.Application.Features.Auth.Commands;

public class LoginUserCommandHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator) 
    : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email, cancellationToken) 
            ?? throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var isPasswordValid = passwordHasher.Verify(request.Password, user.PasswordHash);
        
        if (!isPasswordValid)
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var token = jwtTokenGenerator.GenerateToken(user);

        return token;
    }
}
using MediatR;
using ECommerce.Domain.Entities;
using ECommerce.Application.Contracts.Persistence; 
using ECommerce.Application.Contracts.Security;

namespace ECommerce.Application.Features.Auth.Commands;

public class RegisterUserCommandHandler(
    IUserRepository userRepository, 
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork)
    : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var emailExists = await userRepository.EmailExistsAsync(request.Email, cancellationToken);
        if (emailExists) throw new Exception("El email ya está registrado.");

        var hashedPassword = passwordHasher.Hash(request.Password);
        
        var user = User.Create(
            email: request.Email,
            name: request.Name,
            passwordHash: hashedPassword,
            role: request.Role
        );

        await userRepository.AddAsync(user, cancellationToken);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return user.Id; 
    }
}
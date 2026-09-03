using FluentValidation;
using ECommerce.Application.Features.Auth.Commands;

namespace ECommerce.Application.Features.Auth.Commands;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("El nombre es obligatorio.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
            
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("El rol es obligatorio.");

        RuleFor(x => x.Email)
            .NotNull().WithMessage("El email no puede ser nulo.");
    }
}
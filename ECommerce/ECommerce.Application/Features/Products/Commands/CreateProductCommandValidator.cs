using FluentValidation;

namespace ECommerce.Application.Features.Products.Commands
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre es requerido")
                .MaximumLength(200).WithMessage("Máximo 200 caracteres");
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio debe ser positivo");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock debe ser un valor no negativo");
        }
    }
}
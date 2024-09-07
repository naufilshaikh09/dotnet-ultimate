using FluentValidation;

namespace dotnet_ultimate.Features.Products.Commands.Create;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(4);
        RuleFor(p => p.Price).GreaterThan(0);
    }
}
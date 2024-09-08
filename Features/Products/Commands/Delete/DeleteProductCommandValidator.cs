using FluentValidation;

namespace dotnet_ultimate.Features.Products.Commands.Delete;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0);
    }
}
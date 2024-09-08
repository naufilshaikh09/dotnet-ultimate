using dotnet_ultimate.Features.Products.Commands.Create;
using FluentValidation;

namespace dotnet_ultimate.Features.Products.Commands.Update;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0);
    }
}
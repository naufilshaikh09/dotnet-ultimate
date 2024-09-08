using FluentValidation;

namespace dotnet_ultimate.Features.Products.Queries.Get;

public class GetProductCommandValidator : AbstractValidator<GetProductQuery>
{
    public GetProductCommandValidator()
    {
        RuleFor(p => p.Id).GreaterThan(0);
    }
}
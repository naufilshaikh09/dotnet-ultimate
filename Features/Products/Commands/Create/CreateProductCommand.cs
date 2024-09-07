using MediatR;

namespace dotnet_ultimate.Features.Products.Commands.Create;

public record CreateProductCommand(string Name, string Description, decimal Price) : IRequest<Guid>;

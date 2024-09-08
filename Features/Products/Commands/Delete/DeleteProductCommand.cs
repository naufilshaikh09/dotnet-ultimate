using MediatR;

namespace dotnet_ultimate.Features.Products.Commands.Delete;

public record DeleteProductCommand(int Id) : IRequest;

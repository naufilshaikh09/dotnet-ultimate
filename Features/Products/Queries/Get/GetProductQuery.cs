using dotnet_ultimate.Features.Products.DTOs;
using MediatR;

namespace dotnet_ultimate.Features.Products.Queries.Get;

public record GetProductQuery(Guid Id) : IRequest<ProductDto>;

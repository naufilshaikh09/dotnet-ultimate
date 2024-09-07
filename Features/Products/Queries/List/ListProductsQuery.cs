using dotnet_ultimate.Features.Products.DTOs;
using MediatR;

namespace dotnet_ultimate.Features.Products.Queries.List;

public record ListProductsQuery : IRequest<List<ProductDto>>;

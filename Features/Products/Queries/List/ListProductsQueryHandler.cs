using dotnet_ultimate.Features.Products.DTOs;
using dotnet_ultimate.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace dotnet_ultimate.Features.Products.Queries.List;

public class ListProductsQueryHandler(AppDbContext context) : IRequestHandler<ListProductsQuery, List<ProductDto>> 
{
    public async Task<List<ProductDto>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
    {
        return await context.Products
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price))
            .ToListAsync(cancellationToken);
    }
}
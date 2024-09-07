using dotnet_ultimate.Features.Products.DTOs;
using dotnet_ultimate.Persistence;
using MediatR;

namespace dotnet_ultimate.Features.Products.Queries.Get;

public class GetProductQueryHandler(AppDbContext context) :
    IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync(request.Id);
        
        if (product == null)
            return null;
        
        return new ProductDto(product.Id, product.Name, product.Description, product.Price);
    }
}
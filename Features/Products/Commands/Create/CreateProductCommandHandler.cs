using dotnet_ultimate.Domain;
using dotnet_ultimate.Features.Products.DTOs;
using dotnet_ultimate.Persistence;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace dotnet_ultimate.Features.Products.Commands.Create;

public class CreateProductCommandHandler(AppDbContext context, IMemoryCache cache, ILogger<ProductDto> logger) 
    : IRequestHandler<CreateProductCommand, int>
{
    public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        var product = new Product(0, command.Name, command.Description, command.Price);
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        
        // invalidate cache for products, as new product is added
        const string cacheKey = "products";
        logger.LogInformation("invalidating cache for key: {CacheKey} from cache.", cacheKey);
        cache.Remove(cacheKey);
        
        return product.Id;
    }
}
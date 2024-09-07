using dotnet_ultimate.Domain;
using dotnet_ultimate.Features.Products.DTOs;
using dotnet_ultimate.Persistence;
using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace dotnet_ultimate.Features.Products.Queries.Get;

public class GetProductQueryHandler(AppDbContext context, IMemoryCache cache, ILogger<ProductDto> logger) 
    : IRequestHandler<GetProductQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"product:{request.Id}";
        logger.LogInformation("fetching data for key: {CacheKey} from cache.", cacheKey);
        
        if (!cache.TryGetValue(cacheKey, out Product? product))
        {
            logger.LogInformation("cache miss. fetching data for key: {CacheKey} from database.", cacheKey);
            
            product = await context.Products.FindAsync(request.Id);
            
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(300))
                .SetPriority(CacheItemPriority.Normal);
            
            logger.LogInformation("setting data for key: {CacheKey} to cache.", cacheKey);
            cache.Set(cacheKey, product, cacheOptions);
        }
        else
        {
            logger.LogInformation("cache hit for key: {CacheKey}.", cacheKey);
        }
        
        return product == null ? null : new ProductDto(product.Id, product.Name, product.Description, product.Price);
    }
}
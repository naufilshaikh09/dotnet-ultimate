using dotnet_ultimate.Domain;
using dotnet_ultimate.Features.Products.Commands.Create;
using dotnet_ultimate.Features.Products.DTOs;
using dotnet_ultimate.Persistence;
using MediatR;

namespace dotnet_ultimate.Features.Products.Commands.Update;

public class UpdateProductCommandHandler(AppDbContext context) : IRequestHandler<UpdateProductCommand, Guid?>
{
    public async Task<Guid?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await context.Products.FindAsync(request.Id);
        if (product == null) return null;

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        
        context.Products.Update(product);
        await context.SaveChangesAsync();
        return product.Id;
    }
}
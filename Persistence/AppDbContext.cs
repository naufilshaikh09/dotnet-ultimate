using dotnet_ultimate.Domain;
using Microsoft.EntityFrameworkCore;

namespace dotnet_ultimate.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        // To create in-memory database:
        // Database.EnsureCreated();
    }
    
    public DbSet<Product> Products { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasKey(p => p.Id);
        modelBuilder.Entity<Product>().HasData(
            new Product(1, "iPhone 15 Pro", "Apple's latest flagship smartphone with a ProMotion display and improved cameras", 999.99m),
            new Product(2, "Dell XPS 15", "Dell's high-performance laptop with a 4K InfinityEdge display", 1899.99m),
            new Product(3, "Sony WH-1000XM4", "Sony's top-of-the-line wireless noise-canceling headphones", 349.99m)
        );
    }

    // To use in-memory database:
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseInMemoryDatabase("productDB");
    // }
}
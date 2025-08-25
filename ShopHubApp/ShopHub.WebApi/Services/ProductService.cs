using Microsoft.EntityFrameworkCore;
using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Common;
using ShopHub.WebApi.Data;

namespace ShopHub.WebApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;

    public ProductService(AppDbContext dbContext, ILogger<ProductService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    public async Task<ServiceResult<Product>> CreateAsync(Product product, CancellationToken cancellationToken = default)
    {
        product.Id = Guid.NewGuid().ToString("N");
        product.CreateOnUtc = DateTime.UtcNow;

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();

        return ServiceResult<Product>.Ok(product);
    }

    public async Task<ServiceResult> DeleteAsync(string productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product is null)
            return ServiceResult.Fail($"Product with ProductId:{productId} not found.");
        
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult<IEnumerable<Product>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await _dbContext.Products.AsNoTracking().ToListAsync();

        return ServiceResult<IEnumerable<Product>>.Ok(products);
    }

    public async Task<ServiceResult<IEnumerable<Product>>> GetByCategoryAsync(ProductCategory category, CancellationToken cancellationToken = default)
    {
        var products = await _dbContext.Products.Where(p => p.Category == category).ToListAsync();

        return ServiceResult<IEnumerable<Product>>.Ok(products);
    }

    public async Task<ServiceResult<Product>> GetByIdAsync(string productId, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);

        return product is not null
             ? ServiceResult<Product>.Ok(product) :
             ServiceResult<Product>.Fail($"Product with {productId} Not found.");
    }

    public async Task<ServiceResult> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext.Products.FirstOrDefaultAsync(p=>p.Id == product.Id);
        if (existing is null)
            return ServiceResult.Fail($"Product with ProductId:{product.Id} not found.");
        existing.Name = product.Name;
        existing.Description = product.Description;
        existing.Category = product.Category;
        existing.Price = product.Price;
        existing.StockQuantity = product.StockQuantity;
        existing.UpdateOnUtc = DateTime.UtcNow;

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> UpdateStockAsync(string productId, int quantityChange, CancellationToken cancellationToken = default)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(p=> p.Id == productId);
        if (product is null)
            return ServiceResult.Fail($"Product with ProductId:{productId} not found.");
        product.StockQuantity += quantityChange;
        product.UpdateOnUtc = DateTime.UtcNow;
        return ServiceResult.Ok();        
    }
}

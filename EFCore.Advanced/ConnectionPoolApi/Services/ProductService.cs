using ConnectionPoolApi.Dtos;
using Microsoft.EntityFrameworkCore;
using ProductCatalogDb;

namespace ConnectionPoolApi.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductService> _logger;

    // compiled Queries
    private static readonly Func<AppDbContext, IAsyncEnumerable<Product>> getAllProducts =
        EF.CompileAsyncQuery((AppDbContext context) =>
                context.Products.AsNoTracking());
    
    private static readonly Func<AppDbContext, Guid, Task<Product?>> getProductById =
        EF.CompileAsyncQuery((AppDbContext context, Guid Id) =>
                context.Products.AsNoTracking().FirstOrDefault(x => x.Id == Id));
    
    private static readonly Func<AppDbContext, decimal, decimal, IAsyncEnumerable<Product>> getProductsByPriceRange =
        EF.CompileAsyncQuery((AppDbContext context, decimal minPrice, decimal maxPrice) =>
                context.Products.AsNoTracking().Where(x => x.Price >= minPrice && x.Price <= maxPrice));


    private static readonly Func<AppDbContext, string, decimal, IAsyncEnumerable<Product>> getComplexQuery =
        EF.CompileAsyncQuery((AppDbContext context, string namePattern, decimal minPrice) =>
            context.Products.AsNoTracking().Where(p => p.Price > minPrice)
                                           .Where(p => p.Name.Contains(namePattern))
                                           .OrderBy(p => p.Price)
                                           .ThenBy(p => p.Name)
                                            .Take(50));
    
    public ProductService(AppDbContext dbContext, ILogger<ProductService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Product> AddProductAsync(ProductDto product)
    {
        var newProduct = new Product
        {
            Id = Guid.NewGuid(),
            Name = product.Name,
            Price = product.Price,
            Quantity = product.Quantity            
        };

        _dbContext.Products.Add(newProduct);
        await _dbContext.SaveChangesAsync();
        return newProduct;
    }

    public Task UpdateProductAsync(Guid Id, ProductDto product)
    {
        var existingProduct = _dbContext.Products.Where(x => x.Id == Id).FirstOrDefault();
        if (existingProduct is null)
        {
            _logger.LogWarning("Product with Id {Id} not found.", Id);
            throw new ApplicationException("Product with Id {Id} not found.");
        }
        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Quantity = product.Quantity;
        _dbContext.Update(existingProduct);
        return _dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(Guid Id)
    {
        var existingProduct = await _dbContext.Products.Where(x => x.Id == Id).FirstOrDefaultAsync();
        if (existingProduct is null)
        {
            _logger.LogWarning("Product with Id {Id} not found.", Id);
            throw new ApplicationException("Product with Id {Id} not found.");
        }
        _dbContext.Remove(existingProduct);
        await _dbContext.SaveChangesAsync();
    }
    // Normal Query Implementations
    public async Task<List<Product>> GetAllProductsAsync()
    {
        return await _dbContext.Products.AsNoTracking().ToListAsync();
    }

     public async Task<Product?> GetProductByIdAsync(Guid Id)
    {
        return await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<List<Product>> GetProductByPriceRangeAsync(decimal minPrice, decimal maxPrice)
    {
        return await _dbContext.Products.AsNoTracking().Where(x => x.Price >= minPrice && x.Price <= maxPrice).ToListAsync();
    }

    // Compiled Query Implementations
    public async Task<List<Product>> GetAllProductsCompiledAsync()
    {
        return await ToListAsync(getAllProducts(_dbContext));
    }  

    public async Task<Product?> GetProductByIdCompiledAsync(Guid Id)
    {
        return await getProductById(_dbContext, Id);
    }   

    public async Task<List<Product>> GetProductsByPriceRangeCompiledAsync(decimal minPrice, decimal maxPrice)
    {
        return await ToListAsync(getProductsByPriceRange(_dbContext, minPrice, maxPrice));
    }
   public async Task<List<Product>> GetProductsComplexQuery(string namePattern, decimal minPrice)
   {
        // Only needs to
        // 1. Use Pre-compiled plan
        // 2. Execute SQL
        return await ToListAsync(getComplexQuery(_dbContext, namePattern, minPrice));
   }

   public async Task<List<Product>> GetProductsNormalQuery(string namePattern, decimal minPrice)
   {
    // Each execution performs these steps
    // 1. Parse the linq expression tree
    // 2. Translate LINQ to SQL
    // 3. Generate execution Plan.
    // 4. Execute SQL
      return await  _dbContext.Products.AsNoTracking()
                                    .Where(p => p.Price > minPrice)
                                    .Where(p => p.Name.Contains(namePattern))
                                    .OrderBy(p => p.Price)
                                    .ThenBy(p => p.Name)
                                    .Take(50).ToListAsync();
   }
    private static async Task<List<T>> ToListAsync<T>(IAsyncEnumerable<T> asyncEnumerable)
    {
        var results = new List<T>();
        await foreach(var value in asyncEnumerable)
        {
            results.Add(value);
        }
        return results;
    }
}

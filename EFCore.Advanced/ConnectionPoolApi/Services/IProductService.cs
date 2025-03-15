using ConnectionPoolApi.Dtos;
using ProductCatalogDb;

namespace ConnectionPoolApi.Services;

public interface IProductService
{
  // Normal Queries
  Task<List<Product>> GetAllProductsAsync();
  Task<Product?> GetProductByIdAsync(Guid Id);
  Task<List<Product>> GetProductByPriceRangeAsync(decimal minPrice, decimal maxPrice);

  // compiled Queries
  Task<List<Product>> GetAllProductsCompiledAsync();
  Task<Product?> GetProductByIdCompiledAsync(Guid Id);
  Task<List<Product>> GetProductsByPriceRangeCompiledAsync(decimal minPrice, decimal maxPrice);

  Task<List<Product>> GetProductsComplexQuery(string namePattern, decimal minPrice);
  Task<List<Product>> GetProductsNormalQuery(string namePattern, decimal minPrice);
  // other operations
  Task<Product> AddProductAsync(ProductDto product);
  Task UpdateProductAsync(Guid Id,ProductDto product);
  Task DeleteProductAsync(Guid Id);   
}

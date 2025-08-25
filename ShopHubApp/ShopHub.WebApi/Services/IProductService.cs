using ShopHub.Contracts.Enums;
using ShopHub.Contracts.Models;
using ShopHub.WebApi.Common;

namespace ShopHub.WebApi.Services;

public interface IProductService
{
    Task<ServiceResult<IEnumerable<Product>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ServiceResult<Product>> GetByIdAsync(string productId, CancellationToken cancellationToken = default);
    Task<ServiceResult<Product>> CreateAsync(Product product, CancellationToken cancellationToken = default);
    Task<ServiceResult> UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<ServiceResult> DeleteAsync(string productId, CancellationToken cancellationToken = default);
    Task<ServiceResult<IEnumerable<Product>>> GetByCategoryAsync(ProductCategory category, CancellationToken cancellationToken = default);
    Task<ServiceResult> UpdateStockAsync(string productId, int quantityChange, CancellationToken cancellationToken = default);
}


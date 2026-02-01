using Decorator.CachingLoggingApi.Models;

namespace Decorator.CachingLoggingApi.Services
{
    public interface IProductCatalogService
    {
        Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct);
    }
}

using Decorator.CachingLoggingApi.Models;

namespace Decorator.CachingLoggingApi.Services
{
    public sealed class ProductCatalogService : IProductCatalogService
    {
        private static readonly IReadOnlyList<Product> _products = new List<Product>
        {
            new Product(Guid.NewGuid(), "Red Sneakers", "Shoes"),
            new Product(Guid.NewGuid(), "Blue Hoodie", "Clothing"),
            new Product(Guid.NewGuid(), "Cap", "Accessories")
        };

        public async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct)
        {
            // Simulate external call latency
            await Task.Delay(400, ct);
            return _products;
        }
    }
}

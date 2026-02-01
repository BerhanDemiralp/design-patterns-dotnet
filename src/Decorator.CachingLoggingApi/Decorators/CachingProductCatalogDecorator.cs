using Decorator.CachingLoggingApi.Models;
using Decorator.CachingLoggingApi.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Decorator.CachingLoggingApi.Decorators
{
    public sealed class CachingProductCatalogDecorator : ProductCatalogDecoratorBase
    {
        private readonly IMemoryCache _cache;
        private const string CacheKey = "products";
        private static readonly TimeSpan Ttl = TimeSpan.FromSeconds(30);

        public CachingProductCatalogDecorator(IProductCatalogService inner, IMemoryCache cache) : base(inner)
        {
            _cache = cache;
        }

        public override async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct)
        {
            if (_cache.TryGetValue(CacheKey, out IReadOnlyList<Product>? cached) && cached is not null)
            {
                return cached;
            }

            var result = await Inner.GetProductsAsync(ct);

            _cache.Set(CacheKey, result, Ttl);
            return result;
        }
    }
}

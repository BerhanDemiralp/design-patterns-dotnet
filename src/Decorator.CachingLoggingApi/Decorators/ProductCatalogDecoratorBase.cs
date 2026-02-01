using Decorator.CachingLoggingApi.Models;
using Decorator.CachingLoggingApi.Services;

namespace Decorator.CachingLoggingApi.Decorators
{
    public abstract class ProductCatalogDecoratorBase : IProductCatalogService
    {
        protected readonly IProductCatalogService Inner;

        protected ProductCatalogDecoratorBase(IProductCatalogService inner)
        {
            Inner = inner;
        }

        public virtual Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct)
        {
            return Inner.GetProductsAsync(ct);
        }
    }
}

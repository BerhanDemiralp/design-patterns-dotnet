using System.Diagnostics;
using Decorator.CachingLoggingApi.Models;
using Decorator.CachingLoggingApi.Services;

namespace Decorator.CachingLoggingApi.Decorators
{
    public sealed class LoggingProductCatalogDecorator : ProductCatalogDecoratorBase
    {
        private readonly ILogger<LoggingProductCatalogDecorator> _logger;

        public LoggingProductCatalogDecorator(
            IProductCatalogService inner,
            ILogger<LoggingProductCatalogDecorator> logger) : base(inner)
        {
            _logger = logger;
        }

        public override async Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct)
        {
            var sw = Stopwatch.StartNew();
            _logger.LogInformation("GetProductsAsync started.");

            var result = await Inner.GetProductsAsync(ct);

            sw.Stop();
            _logger.LogInformation("GetProductsAsync finished in {Elapsed} ms. Count={Count}",
                sw.ElapsedMilliseconds, result.Count);

            return result;
        }
    }
}

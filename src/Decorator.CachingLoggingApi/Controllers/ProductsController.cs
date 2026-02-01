using Decorator.CachingLoggingApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Decorator.CachingLoggingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IProductCatalogService _service;

        public ProductsController(IProductCatalogService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken ct)
        {
            var products = await _service.GetProductsAsync(ct);
            return Ok(products);
        }
    }
}

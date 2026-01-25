using Factory.PaymentsApi.Factory;
using Factory.PaymentsApi.Models;
using Factory.PaymentsApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace Factory.PaymentsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class PaymentsController : ControllerBase
    {
        private readonly PaymentService _service;
        private readonly PaymentProviderFactory _factory;

        public PaymentsController(PaymentService service, PaymentProviderFactory factory)
        {
            _service = service;
            _factory = factory;
        }

        [HttpPost]
        public IActionResult Charge([FromBody] PaymentRequest request)
        {
            try
            {
                var result = _service.Charge(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message,
                    availableProviders = _factory.AvailableProviders
                });
            }
        }
    }
}

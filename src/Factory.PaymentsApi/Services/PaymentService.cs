using Factory.PaymentsApi.Factory;
using Factory.PaymentsApi.Models;

namespace Factory.PaymentsApi.Services
{
    public sealed class PaymentService
    {
        private readonly PaymentProviderFactory _factory;

        public PaymentService(PaymentProviderFactory factory)
        {
            _factory = factory;
        }

        public PaymentResult Charge(PaymentRequest request)
        {
            var provider = _factory.Create(request.Provider);
            return provider.Charge(request);
        }
    }
}

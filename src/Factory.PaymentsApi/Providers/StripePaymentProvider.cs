using Factory.PaymentsApi.Models;

namespace Factory.PaymentsApi.Providers
{
    public sealed class StripePaymentProvider : IPaymentProvider
    {
        public string Name => "stripe";

        public PaymentResult Charge(PaymentRequest request)
        {
            return new PaymentResult(
                Provider: Name,
                TransactionId: Guid.NewGuid().ToString("N"),
                Status: "success"
            );
        }
    }
}
using Factory.PaymentsApi.Models;

namespace Factory.PaymentsApi.Providers
{
    public sealed class PayPalPaymentProvider : IPaymentProvider
    {
        public string Name => "paypal";

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

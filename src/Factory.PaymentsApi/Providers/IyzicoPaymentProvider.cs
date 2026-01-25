using Factory.PaymentsApi.Models;

namespace Factory.PaymentsApi.Providers
{
    public sealed class IyzicoPaymentProvider : IPaymentProvider
    {
        public string Name => "iyzico";

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

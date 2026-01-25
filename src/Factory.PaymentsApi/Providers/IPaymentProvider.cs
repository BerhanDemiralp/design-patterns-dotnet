using Factory.PaymentsApi.Models;

namespace Factory.PaymentsApi.Providers
{
    public interface IPaymentProvider
    {
        string Name { get; }
        PaymentResult Charge(PaymentRequest request);
    }
}

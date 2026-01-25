namespace Factory.PaymentsApi.Models
{
    public sealed record PaymentRequest(
        string Provider,
        decimal Amount,
        string Currency
    );
}

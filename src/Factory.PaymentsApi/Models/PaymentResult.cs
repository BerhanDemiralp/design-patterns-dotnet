namespace Factory.PaymentsApi.Models
{
    public sealed record PaymentResult(
        string Provider,
        string TransactionId,
        string Status
    );
}

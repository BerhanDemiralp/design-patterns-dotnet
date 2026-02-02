namespace Command.BackgroundJobsApi.Models
{
    public sealed record SendSmsRequest(
        string PhoneNumber,
        string Message
    );
}

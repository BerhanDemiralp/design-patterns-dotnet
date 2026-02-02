namespace Command.BackgroundJobsApi.Models
{
    public sealed record UserRegisteredRequest(
        string Email,
        string PhoneNumber,
        string FullName
    );
}

namespace Command.BackgroundJobsApi.Models
{
    public sealed record SendEmailRequest(
        string To,
        string Subject,
        string Body
    );
}

namespace Command.BackgroundJobsApi.Commands
{
    public sealed record SendEmailCommand(
        Guid JobId,
        string To,
        string Subject,
        string Body
    ) : ICommand;
}

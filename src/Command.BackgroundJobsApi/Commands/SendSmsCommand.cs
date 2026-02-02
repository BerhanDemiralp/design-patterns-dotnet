namespace Command.BackgroundJobsApi.Commands
{
    public sealed record SendSmsCommand(
        Guid JobId,
        string PhoneNumber,
        string Message
    ) : ICommand;
}

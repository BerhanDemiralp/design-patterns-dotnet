namespace Command.BackgroundJobsApi.Commands
{
    public sealed record UserRegisteredCommand(
        Guid JobId,
        string Email,
        string PhoneNumber,
        string FullName
    ) : ICommand;
}

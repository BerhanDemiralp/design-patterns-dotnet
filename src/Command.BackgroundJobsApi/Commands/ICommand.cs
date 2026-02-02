namespace Command.BackgroundJobsApi.Commands
{
    public interface ICommand
    {
        Guid JobId { get; }
    }
}

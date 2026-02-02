using Command.BackgroundJobsApi.Commands;

namespace Command.BackgroundJobsApi.Infrastructure
{
    public interface ICommandQueue
    {
        void Enqueue(ICommand command);
        Task<ICommand> DequeueAsync(CancellationToken ct);
    }
}

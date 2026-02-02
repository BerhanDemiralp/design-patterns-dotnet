using System.Threading.Channels;
using Command.BackgroundJobsApi.Commands;

namespace Command.BackgroundJobsApi.Infrastructure
{
    public sealed class InMemoryCommandQueue : ICommandQueue
    {
        private readonly Channel<ICommand> _channel = Channel.CreateUnbounded<ICommand>();

        public void Enqueue(ICommand command)
        {
            _channel.Writer.TryWrite(command);
        }

        public async Task<ICommand> DequeueAsync(CancellationToken ct)
        {
            return await _channel.Reader.ReadAsync(ct);
        }
    }
}

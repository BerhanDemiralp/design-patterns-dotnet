using Command.BackgroundJobsApi.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Command.BackgroundJobsApi.Services
{
    public sealed class CommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task DispatchAsync(ICommand command, CancellationToken ct)
        {
            // Resolve the appropriate handler using the command runtime type.
            // This is the key part: we do not switch/if on command names.
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            var handler = _serviceProvider.GetRequiredService(handlerType);

            // Invoke HandleAsync via reflection (minimal, fine for demo)
            var method = handlerType.GetMethod("HandleAsync");
            if (method is null)
                throw new InvalidOperationException("HandleAsync method not found on handler.");

            return (Task)method.Invoke(handler, new object[] { command, ct })!;
        }
    }
}

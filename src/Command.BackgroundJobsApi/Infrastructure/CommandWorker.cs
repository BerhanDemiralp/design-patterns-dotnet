using Command.BackgroundJobsApi.Commands;
using Command.BackgroundJobsApi.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Command.BackgroundJobsApi.Infrastructure
{
    public sealed class CommandWorker : BackgroundService
    {
        private readonly ICommandQueue _queue;
        private readonly JobStore _jobStore;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<CommandWorker> _logger;

        public CommandWorker(
            ICommandQueue queue,
            JobStore jobStore,
            IServiceScopeFactory scopeFactory,
            ILogger<CommandWorker> logger)
        {
            _queue = queue;
            _jobStore = jobStore;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("CommandWorker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                ICommand command = await _queue.DequeueAsync(stoppingToken);

                try
                {
                    _jobStore.MarkProcessing(command.JobId);

                    // Create a scope to resolve scoped services safely
                    using var scope = _scopeFactory.CreateScope();
                    var dispatcher = scope.ServiceProvider.GetRequiredService<CommandDispatcher>();

                    await dispatcher.DispatchAsync(command, stoppingToken);

                    _jobStore.MarkSucceeded(command.JobId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Command execution failed. JobId={JobId}", command.JobId);
                    _jobStore.MarkFailed(command.JobId, ex.Message);
                }
            }
        }
    }
}

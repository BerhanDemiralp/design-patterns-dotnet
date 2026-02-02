namespace Command.BackgroundJobsApi.Handlers
{
    using Command.BackgroundJobsApi.Commands;
    using Microsoft.Extensions.Logging;

    public sealed class GenerateReportCommandHandler : ICommandHandler<GenerateReportCommand>
    {
        private readonly ILogger<GenerateReportCommandHandler> _logger;

        public GenerateReportCommandHandler(ILogger<GenerateReportCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(GenerateReportCommand command, CancellationToken ct)
        {
            _logger.LogInformation(
                "Generating report. JobId={JobId}, Type={Type}, From={From}, To={To}",
                command.JobId, command.ReportType, command.From, command.To);

            // Simulate a longer CPU/IO job
            await Task.Delay(1500, ct);

            _logger.LogInformation("Report generated successfully. JobId={JobId}", command.JobId);
        }
    }
}

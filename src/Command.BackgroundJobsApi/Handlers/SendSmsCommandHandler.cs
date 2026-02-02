namespace Command.BackgroundJobsApi.Handlers
{
    using Command.BackgroundJobsApi.Commands;
    using Microsoft.Extensions.Logging;

    public sealed class SendSmsCommandHandler : ICommandHandler<SendSmsCommand>
    {
        private readonly ILogger<SendSmsCommandHandler> _logger;

        public SendSmsCommandHandler(ILogger<SendSmsCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(SendSmsCommand command, CancellationToken ct)
        {
            _logger.LogInformation(
                "Sending SMS. JobId={JobId}, Phone={Phone}, MessageLength={Len}",
                command.JobId, command.PhoneNumber, command.Message.Length);

            // Simulate external provider latency
            await Task.Delay(600, ct);

            _logger.LogInformation("SMS sent successfully. JobId={JobId}", command.JobId);
        }
    }
}

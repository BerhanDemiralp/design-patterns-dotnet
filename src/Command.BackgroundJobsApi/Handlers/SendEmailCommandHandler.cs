using Command.BackgroundJobsApi.Commands;
using Microsoft.Extensions.Logging;

namespace Command.BackgroundJobsApi.Handlers
{
    public sealed class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
    {
        private readonly ILogger<SendEmailCommandHandler> _logger;

        public SendEmailCommandHandler(ILogger<SendEmailCommandHandler> logger)
        {
            _logger = logger;
        }

        public async Task HandleAsync(SendEmailCommand command, CancellationToken ct)
        {
            _logger.LogInformation(
                "Sending email. JobId={JobId}, To={To}, Subject={Subject}",
                command.JobId, command.To, command.Subject);

            await Task.Delay(800, ct);

            _logger.LogInformation("Email sent successfully. JobId={JobId}", command.JobId);
        }
    }
}

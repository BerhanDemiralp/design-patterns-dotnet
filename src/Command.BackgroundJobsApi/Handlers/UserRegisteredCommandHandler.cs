namespace Command.BackgroundJobsApi.Handlers
{
    using Command.BackgroundJobsApi.Commands;
    using Command.BackgroundJobsApi.Infrastructure;
    using Command.BackgroundJobsApi.Services;
    using Microsoft.Extensions.Logging;

    public sealed class UserRegisteredCommandHandler : ICommandHandler<UserRegisteredCommand>
    {
        private readonly ICommandQueue _queue;
        private readonly JobStore _jobStore;
        private readonly ILogger<UserRegisteredCommandHandler> _logger;

        public UserRegisteredCommandHandler(
            ICommandQueue queue,
            JobStore jobStore,
            ILogger<UserRegisteredCommandHandler> logger)
        {
            _queue = queue;
            _jobStore = jobStore;
            _logger = logger;
        }

        public Task HandleAsync(UserRegisteredCommand command, CancellationToken ct)
        {
            _logger.LogInformation(
                "User registered received. JobId={JobId}, Email={Email}, Phone={Phone}",
                command.JobId, command.Email, command.PhoneNumber);

            // 1) Create a child job for SMS
            var smsJob = _jobStore.Create(commandName: nameof(SendSmsCommand));
            var smsCommand = new SendSmsCommand(
                JobId: smsJob.Id,
                PhoneNumber: command.PhoneNumber,
                Message: $"Welcome {command.FullName}! Your account is ready."
            );
            _queue.Enqueue(smsCommand);

            // 2) (Optional) Create a child job for Email
            var emailJob = _jobStore.Create(commandName: nameof(SendEmailCommand));
            var emailCommand = new SendEmailCommand(
                JobId: emailJob.Id,
                To: command.Email,
                Subject: "Welcome!",
                Body: $"Hi {command.FullName}, welcome to our platform."
            );
            _queue.Enqueue(emailCommand);

            _logger.LogInformation(
                "Enqueued welcome notifications. SmsJobId={SmsJobId}, EmailJobId={EmailJobId}",
                smsJob.Id, emailJob.Id);

            return Task.CompletedTask;
        }
    }
}

namespace Command.BackgroundJobsApi.Controllers
{
    using Command.BackgroundJobsApi.Commands;
    using Command.BackgroundJobsApi.Infrastructure;
    using Command.BackgroundJobsApi.Models;
    using Command.BackgroundJobsApi.Services;
    using Microsoft.AspNetCore.Mvc;

    [ApiController]
    [Route("api/[controller]")]
    public sealed class JobsController : ControllerBase
    {
        private readonly ICommandQueue _queue;
        private readonly JobStore _jobStore;

        public JobsController(ICommandQueue queue, JobStore jobStore)
        {
            _queue = queue;
            _jobStore = jobStore;
        }

        [HttpPost("email")]
        public IActionResult EnqueueEmail([FromBody] SendEmailRequest request)
        {
            // Create a job record (queued)
            var job = _jobStore.Create(commandName: "SendEmailCommand");

            // Create the command with the job id
            var command = new SendEmailCommand(
                JobId: job.Id,
                To: request.To,
                Subject: request.Subject,
                Body: request.Body
            );

            // Enqueue for background processing
            _queue.Enqueue(command);

            // Return 202 Accepted with job id
            return Accepted(new
            {
                jobId = job.Id,
                statusUrl = $"/api/jobs/{job.Id}"
            });
        }

        [HttpPost("sms")]
        public IActionResult EnqueueSms([FromBody] SendSmsRequest request)
        {
            var job = _jobStore.Create(commandName: "SendSmsCommand");

            var command = new SendSmsCommand(
                JobId: job.Id,
                PhoneNumber: request.PhoneNumber,
                Message: request.Message
            );

            _queue.Enqueue(command);

            return Accepted(new
            {
                jobId = job.Id,
                statusUrl = $"/api/jobs/{job.Id}"
            });
        }

        [HttpPost("report")]
        public IActionResult EnqueueReport([FromBody] GenerateReportRequest request)
        {
            var job = _jobStore.Create(commandName: "GenerateReportCommand");

            var command = new GenerateReportCommand(
                JobId: job.Id,
                ReportType: request.ReportType,
                From: request.From,
                To: request.To
            );

            _queue.Enqueue(command);

            return Accepted(new
            {
                jobId = job.Id,
                statusUrl = $"/api/jobs/{job.Id}"
            });
        }

        [HttpPost("user-registered")]
        public IActionResult EnqueueUserRegistered([FromBody] UserRegisteredRequest request)
        {
            var job = _jobStore.Create(commandName: nameof(UserRegisteredCommand));

            var command = new UserRegisteredCommand(
                JobId: job.Id,
                Email: request.Email,
                PhoneNumber: request.PhoneNumber,
                FullName: request.FullName
            );

            _queue.Enqueue(command);

            return Accepted(new
            {
                jobId = job.Id,
                statusUrl = $"/api/jobs/{job.Id}"
            });
        }

        [HttpGet("{id:guid}")]
        public IActionResult GetStatus(Guid id)
        {
            if (!_jobStore.TryGet(id, out var job) || job is null)
                return NotFound(new { error = "Job not found." });

            return Ok(job);
        }
    }
}

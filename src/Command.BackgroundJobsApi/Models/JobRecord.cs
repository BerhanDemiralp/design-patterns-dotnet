namespace Command.BackgroundJobsApi.Models
{
    public sealed class JobRecord
    {
        public Guid Id { get; init; }
        public JobStatus Status { get; set; }
        public string CommandName { get; init; } = string.Empty;
        public string? Error { get; set; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset? CompletedAt { get; set; }
    }
}

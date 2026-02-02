namespace Command.BackgroundJobsApi.Models
{
    public sealed record GenerateReportRequest(
        string ReportType,
        DateTimeOffset From,
        DateTimeOffset To
    );
}

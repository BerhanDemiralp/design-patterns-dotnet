namespace Command.BackgroundJobsApi.Commands
{
    public sealed record GenerateReportCommand(
        Guid JobId,
        string ReportType,
        DateTimeOffset From,
        DateTimeOffset To
    ) : ICommand;
}

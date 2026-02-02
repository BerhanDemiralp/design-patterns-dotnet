using System.Collections.Concurrent;
using Command.BackgroundJobsApi.Models;

namespace Command.BackgroundJobsApi.Services
{
    public sealed class JobStore
    {
        private readonly ConcurrentDictionary<Guid, JobRecord> _jobs = new();

        public JobRecord Create(string commandName)
        {
            var job = new JobRecord
            {
                Id = Guid.NewGuid(),
                Status = JobStatus.Queued,
                CommandName = commandName,
                CreatedAt = DateTimeOffset.UtcNow
            };

            _jobs[job.Id] = job;
            return job;
        }

        public bool TryGet(Guid id, out JobRecord? record)
        {
            return _jobs.TryGetValue(id, out record);
        }

        public void MarkProcessing(Guid id)
        {
            if (_jobs.TryGetValue(id, out var job))
                job.Status = JobStatus.Processing;
        }

        public void MarkSucceeded(Guid id)
        {
            if (_jobs.TryGetValue(id, out var job))
            {
                job.Status = JobStatus.Succeeded;
                job.CompletedAt = DateTimeOffset.UtcNow;
            }
        }

        public void MarkFailed(Guid id, string error)
        {
            if (_jobs.TryGetValue(id, out var job))
            {
                job.Status = JobStatus.Failed;
                job.Error = error;
                job.CompletedAt = DateTimeOffset.UtcNow;
            }
        }
    }
}
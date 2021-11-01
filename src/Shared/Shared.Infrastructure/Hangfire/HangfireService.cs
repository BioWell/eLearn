using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;

namespace Shared.Infrastructure.Hangfire
{
    public class HangfireService : IJobService
    {
        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
    }
}
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Hangfire
{
    public interface IJobService
    {
        string Enqueue(Expression<Func<Task>> methodCall);
    }
}
using System;

namespace Shared.Infrastructure.Caching
{
    public interface ICacheable
    {
        bool BypassCache { get; }

        string CacheKey { get; }

        TimeSpan? SlidingExpiration { get; }
    }
}
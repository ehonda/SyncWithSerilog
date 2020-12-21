using SyncWithSerilog.Filters;
using System;

namespace SyncWithSerilog.Requests
{
    public record ArticleSynchronizationRequest(int Count, double SuccessRate)
    {
        public static ArticleSynchronizationRequest FromFilter(
            ArticleSynchronizationRequestFilter filter)
            => filter switch
            {
                null => throw new ArgumentNullException(nameof(filter)),
                var (count, successRate) => new(count ?? 0, successRate ?? .5)
            };
    }
}

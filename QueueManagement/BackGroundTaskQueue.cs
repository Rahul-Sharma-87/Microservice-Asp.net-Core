using System;
using System.Threading;
using System.Threading.Tasks;

namespace QueueManagement {
    public interface IBackgroundTaskQueue
    {
        void InitializeQueue();

        Task<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken);
    }
}

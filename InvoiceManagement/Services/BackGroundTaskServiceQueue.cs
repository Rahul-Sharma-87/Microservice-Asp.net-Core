using QueueManagement;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceManagement.Services
{
    public class BackGroundTaskServiceQueue: IBackgroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems =
            new ConcurrentQueue<Func<CancellationToken, Task>>();

        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public BackGroundTaskServiceQueue() {
            InitializeQueue();
        }

        public void InitializeQueue() {
            _workItems.Enqueue(EmailingService.SendEmail);
            _workItems.Enqueue(SmsService.SendSms);
        }

        public async Task<Func<CancellationToken, Task>> DequeueAsync(
            CancellationToken cancellationToken)
        {
            _workItems.TryDequeue(out var workItem);
            return workItem;
        }

        
    }
}

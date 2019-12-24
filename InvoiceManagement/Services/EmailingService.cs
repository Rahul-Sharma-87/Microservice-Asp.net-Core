using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InvoiceManagement.Services
{
    public static class EmailingService {
        public static Task SendEmail(CancellationToken stopToken)
        {
            return Task.CompletedTask;
        }
    }
}

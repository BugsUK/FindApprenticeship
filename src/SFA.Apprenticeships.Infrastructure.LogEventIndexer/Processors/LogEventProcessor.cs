namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer.Processors
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using SFA.Infrastructure.Interfaces;
    using Microsoft.ServiceBus.Messaging;
    using Newtonsoft.Json;
    using Services;
    using System.Diagnostics;

    using SFA.Apprenticeships.Application.Interfaces;

    // TODO: LOGGING: fallback logging
    // TODO: LOGGING: Info as Debug.

    // Implementation based on https://azure.microsoft.com/en-gb/documentation/articles/event-hubs-csharp-ephcs-getstarted/.

    public class LogEventProcessor : IEventProcessor
    {
        private readonly ILogService _logService;
        private readonly ILogEventIndexerService _indexerService;

        public LogEventProcessor(
            ILogService logService,
            ILogEventIndexerService indexerService)
        {
            _logService = logService;
            _indexerService = indexerService;
        }

        public Task OpenAsync(PartitionContext context)
        {
            var message = string.Format("OpenAsync: Partition \"{0}\"",
                context.Lease.PartitionId);

            _logService.Info(message);

            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            if (messages == null)
            {
                return;
            }

            var messageCount = 0;

            foreach (var message in messages)
            {
                var bytes = message.GetBytes();
                var json = Encoding.UTF8.GetString(bytes);

                _indexerService.Index(json);

                messageCount++;
            }

            await context.CheckpointAsync();
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            var message = string.Format("CloseAsync: Partition \"{0}\", Reason: \"{1}\"",
                context.Lease.PartitionId, reason);

            _logService.Debug(message);

            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }
    }
}

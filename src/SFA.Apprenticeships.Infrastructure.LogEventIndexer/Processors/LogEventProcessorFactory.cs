namespace SFA.Apprenticeships.Infrastructure.LogEventIndexer.Processors
{
    using SFA.Infrastructure.Interfaces;
    using Microsoft.ServiceBus.Messaging;
    using Services;

    public class LogEventProcessorFactory : IEventProcessorFactory
    {
        private readonly ILogService _logService;
        private readonly LogEventIndexerService _indexerService;

        public LogEventProcessorFactory(
            ILogService logService,
            LogEventIndexerService indexerService)
        {
            _logService = logService;
            _indexerService = indexerService;
        }

        public IEventProcessor CreateEventProcessor(PartitionContext context)
        {
            return new LogEventProcessor(_logService, _indexerService);
        }
    }
}
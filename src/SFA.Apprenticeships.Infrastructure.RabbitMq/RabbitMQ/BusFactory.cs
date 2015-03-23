namespace SFA.Apprenticeships.Infrastructure.RabbitMq.RabbitMQ
{
    using Common.Configuration;
    using EasyNetQ;
    using Configuration;
    using Interfaces;
    using Logging;
    using ServiceOverrides;

    internal class BusFactory
    {
        protected static readonly IRabbitMqServiceProvider CustomServiceProvider;

        static BusFactory()
        {
            CustomServiceProvider = new CustomServiceProvider();
        }

        public static IBus CreateBus()
        {
            //TODO: Figure out how to use IoC here.
            var configurationService = new ConfigurationService(new ConfigurationManager(), new NLogLogService(typeof(BusFactory)));
            var rabbitHost = configurationService.Get<RabbitConfiguration>(RabbitConfiguration.RabbitConfigurationName).MessagingHost;

            var rabbitBus = RabbitHutch.CreateBus(
                                    rabbitHost.ConnectionString, 
                                    CustomServiceProvider.RegisterCustomServices());
            return rabbitBus;
        }
    }
}

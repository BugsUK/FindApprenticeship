namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IoC
{
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using EasyNetQ;
    using Configuration;
    using Interfaces;
    using RabbitMQ;
    using ServiceOverrides;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class RabbitMqRegistry : Registry
    {
        public RabbitMqRegistry()
        {
            For<IBus>().Singleton().Use(context => CreateBus(context));
            For<IBootstrapSubcribers>().Singleton().Use<BootstrapSubcribers>();
            For<IMessageBus>().Singleton().Use<RabbitMessageBus>();
        }

        private IBus CreateBus(IContext context)
        {
            var  customServiceProvider = new CustomServiceProvider();
            var configurationService = context.GetInstance<IConfigurationService>();
            var rabbitHost = configurationService.Get<RabbitConfiguration>(RabbitConfiguration.RabbitConfigurationName).MessagingHost;
            var rabbitBus = RabbitHutch.CreateBus(rabbitHost.ConnectionString, customServiceProvider.RegisterCustomServices());
            return rabbitBus;
        }
    }
}

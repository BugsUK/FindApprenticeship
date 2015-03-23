namespace SFA.Apprenticeships.Infrastructure.RabbitMq.IoC
{
    using System.Runtime.Remoting.Messaging;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Messaging;
    using EasyNetQ;
    using Configuration;
    using Interfaces;
    using RabbitMQ;
    using StructureMap.Configuration.DSL;

    public class RabbitMqRegistry : Registry
    {
        public RabbitMqRegistry()
        {
            For<IBus>()
                .Singleton()
                .Use(BusFactory.CreateBus());

            For<IBootstrapSubcribers>().Singleton().Use<BootstrapSubcribers>();
            For<IMessageBus>().Singleton().Use<RabbitMessageBus>();
        }
    }
}

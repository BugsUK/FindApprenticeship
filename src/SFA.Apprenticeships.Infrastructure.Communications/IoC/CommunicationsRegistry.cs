namespace SFA.Apprenticeships.Infrastructure.Communications.IoC
{
    using Application.Communications;
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class CommunicationsRegistry : Registry
    {
        public CommunicationsRegistry()
        {
            For<CommunicationsControlQueueConsumer>().Use<CommunicationsControlQueueConsumer>();
            For<ICommunicationProcessor>().Use<CommunicationProcessor>();
        }
    }
}

namespace SFA.Apprenticeships.Infrastructure.Migrate.Faa.IoC
{
    using Consumers;
    using StructureMap.Configuration.DSL;

    public class JobsRegistry : Registry
    {
        public JobsRegistry()
        {
            For<FaaMigrationControlQueueConsumer>().Use<FaaMigrationControlQueueConsumer>();
        }
    }
}
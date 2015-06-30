namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC
{
    using Application.Interfaces.Logging;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using StructureMap.Configuration.DSL;

    public class ElasticsearchCommonRegistry : Registry
    {
        public ElasticsearchCommonRegistry()
        {
            For<IElasticsearchClientFactory>()
                .Singleton()
                .Use(context => new ElasticsearchClientFactory(context.GetInstance<IConfigurationService>(), context.GetInstance<ILogService>()));
        }
    }
}

namespace SFA.Apprenticeships.Infrastructure.Elastic.Common.IoC
{
    using SFA.Infrastructure.Interfaces;
    using Configuration;

    using SFA.Apprenticeships.Application.Interfaces;

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

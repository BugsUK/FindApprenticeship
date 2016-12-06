namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Configuration;
    using Apprenticeships.Infrastructure.Repositories.Sql.IoC;
    using StructureMap;

    public static class IoC
    {
        public static IContainer Initialize()
        {
            var container = new Container(x =>
            {
                x.AddRegistry<CommonRegistry>();
                x.AddRegistry<LoggingRegistry>();
            });

            var configurationService = container.GetInstance<IConfigurationService>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            return new Container(c =>
            {
                c.AddRegistry(new RepositoriesRegistry(sqlConfiguration));

                c.AddRegistry<RaaRegistry>();
            });
        }
    }
}
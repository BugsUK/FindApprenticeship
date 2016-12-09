namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using Apprenticeships.Application.Employer.IoC;
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.VacancyPosting.IoC;
    using Apprenticeships.Infrastructure.Common.Configuration;
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Configuration;
    using Apprenticeships.Infrastructure.Repositories.Sql.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider.IoC;
    using Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.IoC;
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
            var cacheConfig = configurationService.Get<CacheConfiguration>();
            var sqlConfiguration = configurationService.Get<SqlConfiguration>();

            return new Container(c =>
            {
                c.AddRegistry(new CommonRegistry(cacheConfig));
                c.AddRegistry<LoggingRegistry>();

                c.AddCachingRegistry(cacheConfig);

                c.AddRegistry(new RepositoriesRegistry(sqlConfiguration));

                c.AddRegistry<VacancyRepositoryRegistry>();
                c.AddRegistry<ProviderRepositoryRegistry>();
                c.AddRegistry<EmployerRepositoryRegistry>();
                c.AddRegistry<VacancyPostingServiceRegistry>();

                c.AddRegistry<EmployerServiceRegistry>();

                c.AddRegistry<RaaRegistry>();
            });
        }
    }
}
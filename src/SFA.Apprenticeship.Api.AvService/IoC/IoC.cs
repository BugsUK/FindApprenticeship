namespace SFA.Apprenticeship.Api.AvService.IoC
{
    using Apprenticeships.Infrastructure.Common.IoC;
    using Apprenticeships.Infrastructure.Logging.IoC;
    using Apprenticeships.Infrastructure.Repositories.Vacancies.IoC;
    using Providers;
    using Providers.Version51;
    using StructureMap;

    // NOTE: WCF IoC strategy is based on this article: https://lostechies.com/jimmybogard/2008/07/30/integrating-structuremap-with-wcf/.
    public static class IoC
    {
        static IoC()
        {
            Container = new Container(x =>
            {
                // Core.
                x.AddRegistry<LoggingRegistry>();
                x.AddRegistry<CommonRegistry>();

                // Vacancies.
                x.AddRegistry<VacancyRepositoryRegistry>();

                // Providers.
                x.For<IVacancyDetailsProvider>().Use<VacancyDetailsProvider>();
            });
        }

        public static IContainer Container { get; private set; }
    }
}

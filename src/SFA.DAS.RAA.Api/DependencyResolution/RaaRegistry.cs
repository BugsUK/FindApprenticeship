namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Infrastructure.Raa.Mappers;
    using Apprenticeships.Infrastructure.Raa.Strategies;
    using Services;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class RaaRegistry : Registry
    {
        public RaaRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IAuthenticationService>().Use<ApiKeyAuthenticationService>();

            For<IPublishVacancySummaryUpdateStrategy>().Use<PublishVacancySummaryUpdateStrategy>().Ctor<IMapper>().Is<VacancySummaryUpdateMapper>();
        }
    }
}
namespace SFA.DAS.RAA.Api.DependencyResolution
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Application.Vacancy;
    using Apprenticeships.Application.VacancyPosting.Strategies;
    using Apprenticeships.Infrastructure.Raa.Mappers;
    using Apprenticeships.Infrastructure.Raa.Strategies;
    using Services;
    using StructureMap.Configuration.DSL;
    using StructureMap.Graph;

    public class RaaApiRegistry : Registry
    {
        public RaaApiRegistry()
        {
            Scan(
                scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                });

            For<IAuthenticationService>().Use<ApiKeyAuthenticationService>();
            For<IVacancySummaryService>().Use<VacancySummaryService>();
            For<IReferenceDataService>().Use<ReferenceDataService>();

            For<IPublishVacancySummaryUpdateStrategy>().Use<PublishVacancySummaryUpdateStrategy>().Ctor<IMapper>().Is<VacancySummaryUpdateMapper>();
        }
    }
}
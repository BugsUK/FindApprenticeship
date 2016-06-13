namespace SFA.Apprenticeships.Web.Candidate.IoC
{
    using Application.Applications;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Infrastructure.Common.Configuration;
    using Infrastructure.LegacyWebServices.Candidate;
    using Infrastructure.LegacyWebServices.GatewayServiceProxy;
    using Infrastructure.LegacyWebServices.Mappers.Apprenticeships;
    using Infrastructure.LegacyWebServices.Mappers.Traineeship;
    using Infrastructure.LegacyWebServices.Vacancy;
    using Infrastructure.LegacyWebServices.Wcf;
    using Infrastructure.Raa;
    using SFA.Infrastructure.Interfaces;
    using SFA.Infrastructure.Interfaces.Caching;
    using StructureMap.Configuration.DSL;

    public class VacancySourceRegistry : Registry
    {
        public VacancySourceRegistry(CacheConfiguration cacheConfiguration, ServicesConfiguration servicesConfiguration)
        {
            // Strategies
            if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Legacy)
            {
                For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<LegacyGetCandidateApprenticeshipApplicationsStrategy>();
                For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
                For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            }
            else if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Raa)
            {
                For<IGetCandidateApprenticeshipApplicationsStrategy>().Use<GetCandidateApprenticeshipApplicationsStrategy>();
                For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<GetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
                For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<GetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            }

            // Services --
            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Legacy)
            {
                For<IMapper>().Use<LegacyApprenticeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyApprenticeshipVacancyDetailMapper";
                For<IMapper>().Use<LegacyTraineeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyTraineeshipVacancyDetailMapper";
            }

            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Legacy 
                || servicesConfiguration.VacanciesSource == ServicesConfiguration.Legacy)
            {
                For<IWcfService<GatewayServiceContract>>().Use<WcfService<GatewayServiceContract>>();
            }

            if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Legacy)
            {
                For<IVacancyIndexDataProvider>()
                    .Use<LegacyVacancyIndexDataProvider>()
                    .Ctor<IMapper>()
                    .Named("LegacyWebServices.LegacyVacancySummaryMapper");

                For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                    .Use<LegacyVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                    .Ctor<IMapper>()
                    .Named("LegacyWebServices.LegacyApprenticeshipVacancyDetailMapper")
                    .Name = "LegacyApprenticeshipVacancyDataProvider";

                For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                    .Use<LegacyVacancyDataProvider<TraineeshipVacancyDetail>>()
                    .Ctor<IMapper>()
                    .Named("LegacyWebServices.LegacyTraineeshipVacancyDetailMapper")
                    .Name = "LegacyTraineeshipVacancyDataProvider";

                if (cacheConfiguration.UseCache)
                {
                    For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                        .Use<CachedLegacyVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                        .Ctor<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                        .IsTheDefault()
                        .Ctor<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                        .Named("LegacyApprenticeshipVacancyDataProvider")
                        .Ctor<ICacheService>()
                        .Named(cacheConfiguration.DefaultCache);

                    For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                        .Use<CachedLegacyVacancyDataProvider<TraineeshipVacancyDetail>>()
                        .Ctor<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                        .IsTheDefault()
                        .Ctor<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                        .Named("LegacyTraineeshipVacancyDataProvider")
                        .Ctor<ICacheService>()
                        .Named(cacheConfiguration.DefaultCache);
                }

                For<ILegacyApplicationStatusesProvider>()
                    .Use<LegacyCandidateApplicationStatusesProvider>()
                    .Ctor<IMapper>()
                    .Named("LegacyWebServices.ApplicationStatusSummaryMapper")
                    .Name = "LegacyCandidateApplicationStatusesProvider";
            }
            else if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Raa)
            {
                For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

                For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                    .Use<ApprenticeshipVacancyDataProvider>();

                For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                    .Use<TraineeshipVacancyDataProvider>();

                For<ILegacyApplicationStatusesProvider>()
                    .Use<NullApplicationStatusesProvider>();
            }

            //--
        }     
    }
}
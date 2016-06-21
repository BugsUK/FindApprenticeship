namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.IoC
{
    using Application.Applications;
    using Application.Candidate.Strategies;
    using Application.Candidate.Strategies.Apprenticeships;
    using Application.Vacancies;
    using Application.Vacancy;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Common.Configuration;
    using LegacyWebServices.Candidate;
    using LegacyWebServices.GatewayServiceProxy;
    using LegacyWebServices.Mappers.Apprenticeships;
    using LegacyWebServices.Mappers.Traineeship;
    using LegacyWebServices.Vacancy;
    using LegacyWebServices.Wcf;
    using Raa;

    using Application.Candidate.Configuration;
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
                For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
                For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<LegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            }
            else if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Raa)
            {
                For<ILegacyGetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>().Use<GetCandidateVacancyDetailStrategy<ApprenticeshipVacancyDetail>>();
                For<ILegacyGetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>().Use<GetCandidateVacancyDetailStrategy<TraineeshipVacancyDetail>>();
            }

            // Application status provider -> it's not exactly related with vacancy sources...
            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Legacy)
            {
                For<IGetCandidateApprenticeshipApplicationsStrategy>()
                    .Use<LegacyGetCandidateApprenticeshipApplicationsStrategy>();

                For<ILegacyApplicationStatusesProvider>()
                    .Use<LegacyCandidateApplicationStatusesProvider>()
                    .Ctor<IMapper>()
                    .Named("LegacyWebServices.ApplicationStatusSummaryMapper")
                    .Name = "LegacyCandidateApplicationStatusesProvider";
            }
            else if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Raa)
            {
                For<IGetCandidateApprenticeshipApplicationsStrategy>()
                    .Use<GetCandidateApprenticeshipApplicationsStrategy>();

                For<ILegacyApplicationStatusesProvider>()
                    .Use<NullApplicationStatusesProvider>();
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
            }
            else if (servicesConfiguration.VacanciesSource == ServicesConfiguration.Raa)
            {
                For<IVacancyIndexDataProvider>().Use<VacancyIndexDataProvider>();

                For<IVacancyDataProvider<ApprenticeshipVacancyDetail>>()
                    .Use<ApprenticeshipVacancyDataProvider>();

                For<IVacancyDataProvider<TraineeshipVacancyDetail>>()
                    .Use<TraineeshipVacancyDataProvider>();
            }

            //--
        }     
    }
}
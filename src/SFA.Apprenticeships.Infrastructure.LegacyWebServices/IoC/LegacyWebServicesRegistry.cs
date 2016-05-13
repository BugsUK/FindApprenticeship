namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using Application;
    using Apprenticeships.Application.Applications;
    using Apprenticeships.Application.Candidate;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Application.Vacancies;
    using Apprenticeships.Application.Vacancy;
    using Candidate;
    using Common.Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using SFA.Infrastructure.Interfaces.Caching;
    using SFA.Infrastructure.Interfaces;
    using GatewayServiceProxy;
    using LegacyReferenceDataProxy;
    using Mappers;
    using Mappers.Apprenticeships;
    using Mappers.Traineeship;
    using ReferenceData;
    using StructureMap.Configuration.DSL;
    using Vacancy;
    using Wcf;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry(ServicesConfiguration servicesConfiguration) : this(new CacheConfiguration(), servicesConfiguration) { }

        public LegacyWebServicesRegistry(CacheConfiguration cacheConfiguration, ServicesConfiguration servicesConfiguration)
        {
            For<IMapper>().Use<LegacyVacancySummaryMapper>().Name = "LegacyWebServices.LegacyVacancySummaryMapper";
            For<IMapper>().Use<LegacyApprenticeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyApprenticeshipVacancyDetailMapper";
            For<IMapper>().Use<LegacyTraineeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyTraineeshipVacancyDetailMapper";
            For<IWcfService<GatewayServiceContract>>().Use<WcfService<GatewayServiceContract>>();
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();

            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Legacy)
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

                #region Reference Data Service and Providers

                For<IReferenceDataProvider>().Use<ReferenceDataProvider>().Name = "LegacyReferenceDataProvider";

                if (cacheConfiguration.UseCache)
                {
                    For<IReferenceDataProvider>()
                        .Use<CachedReferenceDataProvider>()
                        .Ctor<IReferenceDataProvider>()
                        .IsTheDefault()
                        .Ctor<IReferenceDataProvider>()
                        .Named("LegacyReferenceDataProvider")
                        .Ctor<ICacheService>()
                        .Named(cacheConfiguration.DefaultCache);
                }

                For<ILegacyApplicationStatusesProvider>()
                    .Use<LegacyCandidateApplicationStatusesProvider>()
                    .Ctor<IMapper>()
                    .Named("LegacyWebServices.ApplicationStatusSummaryMapper")
                    .Name = "LegacyCandidateApplicationStatusesProvider";

                #endregion
            }

            #region Candidate and Application Providers

            For<ILegacyCandidateProvider>().Use<LegacyCandidateProvider>();

            For<ILegacyApplicationProvider>().Use<LegacyApplicationProvider>();

            For<IMapper>().Use<ApplicationStatusSummaryMapper>()
                .Name = "LegacyWebServices.ApplicationStatusSummaryMapper";

            #endregion
        }
    }
}
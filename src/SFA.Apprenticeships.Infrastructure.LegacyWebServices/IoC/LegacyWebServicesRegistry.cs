namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using Application;
    using Apprenticeships.Application.Applications;
    using Apprenticeships.Application.Candidate;
    using Apprenticeships.Application.ReferenceData;
    using Apprenticeships.Application.ReferenceData.Configuration;
    using Apprenticeships.Application.Vacancies;
    using Apprenticeships.Application.Vacancy;
    using Candidate;
    using Common.Configuration;
    using Domain.Entities.Vacancies.Apprenticeships;
    using Domain.Entities.Vacancies.Traineeships;
    using Domain.Interfaces.Caching;
    using Domain.Interfaces.Mapping;
    using GatewayServiceProxy;
    using LegacyReferenceDataProxy;
    using Mappers;
    using Mappers.Apprenticeships;
    using Mappers.Traineeship;
    using ReferenceData;
    using StructureMap.Configuration.DSL;
    using TacticalDataServices;
    using Vacancy;
    using Wcf;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry() : this(new CacheConfiguration(), new ReferenceDataConfiguration { Provider = "LegacyReferenceDataProvider" }) { }

        public LegacyWebServicesRegistry(CacheConfiguration cacheConfiguration, ReferenceDataConfiguration referenceDataConfiguration)
        {
            For<IMapper>().Use<LegacyVacancySummaryMapper>().Name = "LegacyWebServices.LegacyVacancySummaryMapper";
            For<IMapper>().Use<LegacyApprenticeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyApprenticeshipVacancyDetailMapper";
            For<IMapper>().Use<LegacyTraineeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyTraineeshipVacancyDetailMapper";
            For<IWcfService<GatewayServiceContract>>().Use<WcfService<GatewayServiceContract>>();
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();

            For<IVacancyIndexDataProvider>()
                .Use<LegacyVacancyIndexDataProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.LegacyVacancySummaryMapper");

            #region Vacancy Data Service And Providers

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

            #endregion

            #region Candidate and Application Providers

            For<ILegacyCandidateProvider>().Use<LegacyCandidateProvider>();

            For<ILegacyApplicationProvider>().Use<LegacyApplicationProvider>();

            For<IMapper>().Use<ApplicationStatusSummaryMapper>()
                .Name = "LegacyWebServices.ApplicationStatusSummaryMapper";

            For<ILegacyApplicationStatusesProvider>()
                .Use<LegacyCandidateApplicationStatusesProvider>()
                .Ctor<IMapper>()
                .Named("LegacyWebServices.ApplicationStatusSummaryMapper")
                .Name = "LegacyCandidateApplicationStatusesProvider";

            #endregion

            #region Reference Data Service and Providers

            if (referenceDataConfiguration.Provider == "FrameworkDataProvider")
            {
                For<IReferenceDataProvider>().Use<FrameworkDataProvider>().Name = referenceDataConfiguration.Provider;
            }
            else
            {
                For<IReferenceDataProvider>().Use<ReferenceDataProvider>().Name = referenceDataConfiguration.Provider;
            }

            if (cacheConfiguration.UseCache)
            {
                For<IReferenceDataProvider>()
                    .Use<CachedReferenceDataProvider>()
                    .Ctor<IReferenceDataProvider>()
                    .IsTheDefault()
                    .Ctor<IReferenceDataProvider>()
                    .Named(referenceDataConfiguration.Provider)
                    .Ctor<ICacheService>()
                    .Named(cacheConfiguration.DefaultCache);
            }

            #endregion

        }
    }
}
namespace SFA.Apprenticeships.Infrastructure.LegacyWebServices.IoC
{
    using Application;
    using Apprenticeships.Application.Candidate;
    using Apprenticeships.Application.ReferenceData;
    using Candidate;
    using Common.Configuration;
    using SFA.Infrastructure.Interfaces;
    using LegacyReferenceDataProxy;
    using Mappers;
    using Mappers.Apprenticeships;
    using Mappers.Traineeship;
    using ReferenceData;
    using Apprenticeships.Application.Candidate.Configuration;
    using SFA.Infrastructure.Interfaces.Caching;
    using StructureMap.Configuration.DSL;
    using Wcf;

    public class LegacyWebServicesRegistry : Registry
    {
        public LegacyWebServicesRegistry(ServicesConfiguration servicesConfiguration, CacheConfiguration cacheConfiguration)
        {
            For<IMapper>().Use<LegacyVacancySummaryMapper>().Name = "LegacyWebServices.LegacyVacancySummaryMapper";
            For<IMapper>().Use<LegacyApprenticeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyApprenticeshipVacancyDetailMapper";
            For<IMapper>().Use<LegacyTraineeshipVacancyDetailMapper>().Name = "LegacyWebServices.LegacyTraineeshipVacancyDetailMapper";
            For<IWcfService<IReferenceData>>().Use<WcfService<IReferenceData>>();

            if (servicesConfiguration.ServiceImplementation == ServicesConfiguration.Legacy)
            {
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
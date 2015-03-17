namespace SFA.Apprenticeships.Infrastructure.ScheduledJobs.IoC
{
    using Application.Applications;
    using Application.Applications.Strategies;
    using Application.Communications;
    using Application.Communications.Strategies;
    using Application.Interfaces.Locations;
    using Application.Location;
    using Application.Vacancies;
    using Consumers;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using StructureMap.Configuration.DSL;

    public class JobsRegistry : Registry
    {
        public JobsRegistry()
        {
            //Application Etl
            For<ApplicationEtlControlQueueConsumer>().Use<ApplicationEtlControlQueueConsumer>();
            For<IApplicationStatusProcessor>().Use<ApplicationStatusProcessor>();
            For<IApplicationStatusUpdateStrategy>().Use<ApplicationStatusUpdateStrategy>();
            For<IApplicationStatusAlertStrategy>().Use<ApplicationStatusAlertStrategy>();

            //Vacancy Etl
            For<VacancyEtlControlQueueConsumer>().Use<VacancyEtlControlQueueConsumer>();
            For<SavedSearchControlQueueConsumer>().Use<SavedSearchControlQueueConsumer>();

            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");

            //Communications
            For<DailyDigestControlQueueConsumer>().Use<DailyDigestControlQueueConsumer>();
            For<ICommunicationProcessor>().Use<CommunicationProcessor>();
            For<ISendDailyDigestsStrategy>().Use<SendDailyDigestsStrategy>();
            For<ISendSavedSearchAlertsStrategy>().Use<SendSavedSearchAlertsStrategy>();

            For<ILocationSearchService>().Use<LocationSearchService>();
            For<ISavedSearchProcessor>().Use<SavedSearchProcessor>();
        }
    }
}
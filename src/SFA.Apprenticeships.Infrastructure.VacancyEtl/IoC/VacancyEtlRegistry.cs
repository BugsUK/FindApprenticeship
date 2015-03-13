namespace SFA.Apprenticeships.Infrastructure.VacancyEtl.IoC
{
    using Application.Vacancies;
    using Consumers;
    using Domain.Interfaces.Mapping;
    using Processes.Vacancies;
    using StructureMap.Configuration.DSL;

    public class VacancyEtlRegistry : Registry
    {
        public VacancyEtlRegistry()
        {
            For<VacancyEtlControlQueueConsumer>().Use<VacancyEtlControlQueueConsumer>();
            For<SavedSearchControlQueueConsumer>().Use<SavedSearchControlQueueConsumer>();
            For<IMapper>().Singleton().Use<VacancyEtlMapper>().Name = "VacancyEtlMapper";
            For<IVacancySummaryProcessor>().Use<VacancySummaryProcessor>().Ctor<IMapper>().Named("VacancyEtlMapper");
            For<VacancyAboutToExpireConsumerAsync>().Use<VacancyAboutToExpireConsumerAsync>().Ctor<IMapper>().Named("VacancyEtlMapper");
        }
    }
}

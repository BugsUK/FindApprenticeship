namespace SFA.Apprenticeships.Application.VacancyPosting.IoC
{
    using Interfaces.Vacancies;
    using Interfaces.VacancyPosting;
    using Strategies;
    using StructureMap.Configuration.DSL;

    public class VacancyPostingServiceRegistry : Registry
    {
        public VacancyPostingServiceRegistry()
        {
            // Strategies
            For<ICreateVacancyStrategy>().Use<CreateVacancyStrategy>();
            For<IUpdateVacancyStrategy>().Use<UpdateVacancyStrategy>();
            For<IArchiveVacancyStrategy>().Use<ArchiveVacancyStrategy>();
            For<IGetNextVacancyReferenceNumberStrategy>().Use<GetNextVacancyReferenceNumberStrategy>();
            For<IGetVacancyStrategies>().Use<GetVacancyStrategies>();
            For<IGetVacancySummaryStrategies>().Use<GetVacancySummaryStrategies>();
            For<IQaVacancyStrategies>().Use<QaVacancyStrategies>();
            For<IVacancyLocationsStrategies>().Use<VacancyLocationsStrategies>();
            For<IUpsertVacancyStrategy>().Use<UpsertVacancyStrategy>();
            For<IAuthoriseCurrentUserStrategy>().Use<AuthoriseCurrentUserStrategy>();

            // Services
            For<IVacancyLockingService>().Use<VacancyLockingService>();
            For<IVacancyPostingService>().Use<VacancyPostingService>();
        }
    }
}
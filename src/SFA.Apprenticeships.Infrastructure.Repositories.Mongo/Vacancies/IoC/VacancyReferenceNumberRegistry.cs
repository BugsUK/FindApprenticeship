namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Vacancies.IoC
{
    using Domain.Interfaces.Repositories;
    using StructureMap.Configuration.DSL;

    // TODO: move to VacancyRepositoryRegistry
    public class VacancyReferenceNumberRegistry : Registry
    {
        public VacancyReferenceNumberRegistry()
        {
            For<IReferenceNumberRepository>()
                .Use<ReferenceNumberRepository>();
        }
    }
}

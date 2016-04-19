namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Entities.Mongo;
    using Entities.Sql;

    public interface IApplicationMappers
    {
        Application MapApplication(VacancyApplication apprenticeshipApplication, Candidate candidate);
        dynamic MapApplicationDynamic(VacancyApplication apprenticeshipApplication, Candidate candidate);
        IDictionary<string, object> MapApplicationDictionary(VacancyApplication apprenticeshipApplication, Candidate candidate);
    }
}
namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;

    public interface IApplicationMappers
    {
        Application MapApplication(VacancyApplication apprenticeshipApplication, Candidate candidate);
        ApplicationWithHistory MapApplicationWithHistory(VacancyApplication apprenticeshipApplication, Candidate candidate);
        IDictionary<string, object> MapApplicationDictionary(VacancyApplication apprenticeshipApplication, Candidate candidate);
        ApplicationWithHistoryDictionary MapApplicationWithHistoryDictionary(VacancyApplication apprenticeshipApplication, Candidate candidate);
    }
}
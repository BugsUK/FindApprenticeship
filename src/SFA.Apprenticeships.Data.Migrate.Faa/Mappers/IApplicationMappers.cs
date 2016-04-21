namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;

    public interface IApplicationMappers
    {
        Application MapApplication(VacancyApplication apprenticeshipApplication, CandidateSummary candidate);
        ApplicationWithHistory MapApplicationWithHistory(VacancyApplication apprenticeshipApplication, CandidateSummary candidate);
        IDictionary<string, object> MapApplicationDictionary(VacancyApplication apprenticeshipApplication, CandidateSummary candidate);
        ApplicationWithHistoryDictionary MapApplicationWithHistoryDictionary(VacancyApplication apprenticeshipApplication, CandidateSummary candidate);
    }
}
namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;

    public interface IApplicationMappers
    {
        Application MapApplication(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds, IDictionary<int, ApplicationSummary> sourceApplicationSummaries);
        ApplicationWithHistory MapApplicationWithHistory(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds, IDictionary<int, ApplicationSummary> sourceApplicationSummaries, IDictionary<int, Dictionary<int, int>> applicationHistoryIds, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries);
        IDictionary<string, object> MapApplicationDictionary(Application application);
    }
}
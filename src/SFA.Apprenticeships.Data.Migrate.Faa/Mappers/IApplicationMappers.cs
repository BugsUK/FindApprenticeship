namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;

    public interface IApplicationMappers
    {
        Application MapApplication(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds);
        ApplicationWithHistory MapApplicationWithHistory(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds, IDictionary<int, Dictionary<int, int>> applicationHistoryIds);
        IDictionary<string, object> MapApplicationDictionary(Application application);
    }
}
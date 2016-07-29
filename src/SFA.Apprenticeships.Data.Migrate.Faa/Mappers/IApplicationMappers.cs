namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;

    public interface IApplicationMappers
    {
        IDictionary<Guid, int> GetApplicationIds(IDictionary<Guid, ApplicationIds> destinationApplicationIds, IDictionary<int, Dictionary<int, ApplicationIds>> candidateApplicationIds, IList<VacancyApplication> vacancyApplications, IDictionary<Guid, int> candidateIds);
        ApplicationWithSubVacancy MapApplication(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds, IDictionary<int, ApplicationSummary> sourceApplicationSummaries, IDictionary<int, int> schoolAttendedIds, IDictionary<int, SubVacancy> subVacancies);
        ApplicationWithHistory MapApplicationWithHistory(VacancyApplication apprenticeshipApplication, int candidateId, IDictionary<Guid, int> applicationIds, IDictionary<int, ApplicationSummary> sourceApplicationSummaries, IDictionary<int, int> schoolAttendedIds, IDictionary<int, SubVacancy> subVacancies, IDictionary<int, Dictionary<int, int>> applicationHistoryIds, IDictionary<int, List<ApplicationHistorySummary>> sourceApplicationHistorySummaries);
        IDictionary<string, object> MapApplicationDictionary(Application application);
        IDictionary<string, object> MapSubVacancyDictionary(SubVacancy subVacancy);
    }
}
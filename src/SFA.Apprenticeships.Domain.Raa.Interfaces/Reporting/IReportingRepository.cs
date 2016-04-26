namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IReportingRepository
    {
        List<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);
        List<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange);
        Dictionary<string, string> LocalAuthorityManagerGroups();
        Dictionary<string, string> GeoRegionsIncludingAll();
    }
}

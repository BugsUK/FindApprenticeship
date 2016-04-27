namespace SFA.Apprenticeships.Application.Interfaces.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Reporting.Models;

    public interface IReportingService
    {
        IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);

        IList<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange);

        IList<ReportVacancyExtensionsResultItem> ReportVacancyExtensions(DateTime fromDate, DateTime toDate,
            int? providerUkprn, int? vacancyStatus);

        Dictionary<string, string> LocalAuthorityManagerGroups();

        Dictionary<string, string> RegionsIncludingAll();
    }
}

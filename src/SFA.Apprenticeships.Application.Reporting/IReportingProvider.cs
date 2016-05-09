namespace SFA.Apprenticeships.Application.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Reporting.Models;

    public interface IReportingProvider
    {
        IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);
        IList<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string reportType, DateTime fromDate, DateTime toDate, string ageRange, string managedBy, string region);
        IList<ReportSuccessfulCandidatesResultItem> ReportSuccessfulCandidates(string reportType, DateTime fromDate, DateTime toDate, string ageRange, string managedBy, string region);
        Dictionary<string, string> LocalAuthorityManagerGroups();
        Dictionary<string, string> RegionsIncludingAll();
        IList<ReportVacancyExtensionsResultItem> ReportVacancyExtensions(DateTime fromDate, DateTime toDate, int? providerUkprn, int? vacancyStatus);
    }
}

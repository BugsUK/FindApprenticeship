namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting
{
    using System;
    using System.Collections.Generic;
    using Entities.Raa.Reporting;
    using Models;

    public interface IReportingRepository
    {
        List<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);
        List<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange, string managedBy, string region);
        List<ReportSuccessfulCandidatesResultItem> ReportSuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange, string managedBy, string region);
        Dictionary<string, string> LocalAuthorityManagerGroups();
        Dictionary<string, string> GeoRegionsIncludingAll();
        Dictionary<string, string> GetLocalAuthorities();
        IList<ReportVacancyExtensionsResultItem> ReportVacancyExtensions(DateTime fromDate, DateTime toDate, int? providerUkprn, int? vacancyStatus);
        IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId);
        IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId);
        InformationRadiatorData GetInformationRadiatorData();
        IList<ReportRegisteredCandidatesResultItem> ReportRegisteredCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange, string region, string localAuthority, bool marketMessagesOnly);
    }
}

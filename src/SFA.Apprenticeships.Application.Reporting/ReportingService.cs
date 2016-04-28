namespace SFA.Apprenticeships.Application.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Reporting.Models;
    using Interfaces.Reporting;

    public class ReportingService : IReportingService
    {
        private readonly IReportingProvider _reportingProvider;

        public ReportingService(IReportingProvider reportingProvider)
        {
            _reportingProvider = reportingProvider;
        }

        public IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            return _reportingProvider.ReportVacanciesList(fromDate, toDate);
        }

        public IList<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string reportType, DateTime fromDate, DateTime toDate, string ageRange, string managedBy, string region)
        {
            return _reportingProvider.ReportUnsuccessfulCandidates(reportType, fromDate, toDate, ageRange, managedBy, region);
        }

        public IList<ReportVacancyExtensionsResultItem> ReportVacancyExtensions(DateTime fromDate, DateTime toDate, int? providerUkprn, int? vacancyStatus)
        {
            return _reportingProvider.ReportVacancyExtensions(fromDate, toDate, providerUkprn, vacancyStatus);
        }

        public IList<ReportSuccessfulCandidatesResultItem> ReportSuccessfulCandidates(string type, DateTime fromDate, DateTime toDate, string ageRange, string managedBy,
            string region)
        {
            return _reportingProvider.ReportSuccessfulCandidates(type, fromDate, toDate, ageRange, managedBy, region);
        }

        public Dictionary<string, string> LocalAuthorityManagerGroups()
        {
            return _reportingProvider.LocalAuthorityManagerGroups();
        }

        public Dictionary<string, string> RegionsIncludingAll()
        {
            return _reportingProvider.RegionsIncludingAll();
        }
    }
}

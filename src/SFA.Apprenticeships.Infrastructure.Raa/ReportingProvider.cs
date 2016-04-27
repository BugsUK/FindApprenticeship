namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System;
    using System.Collections.Generic;
    using Application.Reporting;
    using Domain.Raa.Interfaces.Reporting;
    using Domain.Raa.Interfaces.Reporting.Models;

    public class ReportingProvider : IReportingProvider
    {
        private IReportingRepository _reportingRepository;

        public ReportingProvider(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            return _reportingRepository.ReportVacanciesList(fromDate, toDate);
        }

        public IList<ReportUnsuccessfulCandidatesResultItem> ReportUnsuccessfulCandidates(string reportType, DateTime fromDate, DateTime toDate, string ageRange)
        {
            return _reportingRepository.ReportUnsuccessfulCandidates(reportType, fromDate, toDate, ageRange);
        }

        public Dictionary<string, string> LocalAuthorityManagerGroups()
        {
            return _reportingRepository.LocalAuthorityManagerGroups();
        }

        public Dictionary<string, string> RegionsIncludingAll()
        {
            return _reportingRepository.GeoRegionsIncludingAll();
        }

        public IList<ReportVacancyExtensionsResultItem> ReportVacancyExtensions(DateTime fromDate, DateTime toDate, int? providerUkprn, int? vacancyStatus)
        {
            return _reportingRepository.ReportVacancyExtensions(fromDate, toDate, providerUkprn, vacancyStatus);
        }
    }
}

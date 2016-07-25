namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Reporting;
    using Domain.Entities.Raa.Reporting;

    public class ReportingProvider : IReportingProvider
    {
        private IReportingService _reportingService;

        public ReportingProvider(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        public IEnumerable<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo)
        {
            return _reportingService.GetApplicationsReceivedResultItems(dateFrom, dateTo);
        }

        public IEnumerable<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItem(DateTime dateFrom, DateTime dateTo)
        {
            return _reportingService.GetCandidatesWithApplicationsResultItems(dateFrom, dateTo);
        }
    }
}
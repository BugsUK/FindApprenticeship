namespace SFA.Apprenticeships.Application.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Reporting.Models;
    using Interfaces.Reporting;

    public class ReportingService : IReportingService
    {
        private IReportingProvider _reportingProvider;

        public ReportingService(IReportingProvider reportingProvider)
        {
            _reportingProvider = reportingProvider;
        }

        public IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            return _reportingProvider.ReportVacanciesList(fromDate, toDate);
        }
    }
}

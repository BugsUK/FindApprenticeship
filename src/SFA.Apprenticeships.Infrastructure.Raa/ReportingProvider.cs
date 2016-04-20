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
    }
}

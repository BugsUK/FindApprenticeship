namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Application.Interfaces.Reporting;
    using Infrastructure.Presentation;

    public class ReportingMediator : IReportingMediator
    {
        private IReportingService _reportingService;

        public ReportingMediator(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        public byte[] ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            var reportResult = _reportingService.ReportVacanciesList(fromDate, toDate);
            var bytes = GetCsvBytes(reportResult);
            return bytes;
        }

        private byte[] GetCsvBytes<T>(IEnumerable<T> items) where T : class
        {
            var csvString = CsvPresenter.ToCsv(items);
            var bytes = ASCIIEncoding.ASCII.GetBytes(csvString);
            return bytes;
        }
    }
}
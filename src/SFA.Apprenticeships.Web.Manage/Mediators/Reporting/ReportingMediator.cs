namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Application.Interfaces.Reporting;
    using Infrastructure.Presentation;
    using ViewModels;

    public class ReportingMediator : IReportingMediator
    {
        private IReportingService _reportingService;

        public ReportingMediator(IReportingService reportingService)
        {
            _reportingService = reportingService;
        }

        public byte[] GetVacanciesListReportBytes(ReportVacanciesParameters parameters)
        {
            var reportResult = _reportingService.ReportVacanciesList(parameters.FromDate, parameters.ToDate);
            var bytes = GetCsvBytes(reportResult);
            return bytes;
        }

        public byte[] GetSuccessfulCandidatesReportBytes(ReportSuccessfulCandidatesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public byte[] GetUnsuccessfulCandidatesReportBytes(ReportUnsuccessfulCandidatesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public byte[] GetVacancyExtensionsReportBytes(ReportVacancyExtensionsParameters parameters)
        {
            throw new NotImplementedException();
        }

        private byte[] GetCsvBytes<T>(IEnumerable<T> items) where T : class
        {
            var csvString = CsvPresenter.ToCsv(items);
            var bytes = ASCIIEncoding.ASCII.GetBytes(csvString);
            return bytes;
        }
    }
}
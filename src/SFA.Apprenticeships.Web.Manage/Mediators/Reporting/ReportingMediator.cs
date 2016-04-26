namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Application.Interfaces.Reporting;
    using Common.Extensions;
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
            var reportResult = _reportingService.ReportUnsuccessfulCandidates(parameters.Type, parameters.FromDate, parameters.ToDate, parameters.AgeRange);
            var bytes = GetCsvBytes(reportResult);
            return bytes;
        }

        public ReportUnsuccessfulCandidatesParameters GetUnsuccessfulCandidatesReportParams()
        {
            var result = new ReportUnsuccessfulCandidatesParameters();
            var localAuthorities = _reportingService.LocalAuthorityManagerGroups();
            result.ManagedByList = localAuthorities.ToListOfListItem();
            var regions = _reportingService.RegionsIncludingAll();
            result.RegionList = regions.ToListOfListItem();
            return result;
        }

        public ReportSuccessfulCandidatesParameters GetSuccessfulCandidatesReportParams()
        {
            var result = new ReportSuccessfulCandidatesParameters();
            var localAuthorities = _reportingService.LocalAuthorityManagerGroups();
            result.ManagedByList = localAuthorities.ToListOfListItem();
            var regions = _reportingService.RegionsIncludingAll();
            result.RegionList = regions.ToListOfListItem();
            return result;
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
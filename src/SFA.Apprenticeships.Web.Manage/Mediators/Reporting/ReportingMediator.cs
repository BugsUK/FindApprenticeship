namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Application.Interfaces.Reporting;
    using Common.Extensions;
    using Common.Mediators;
    using Infrastructure.Presentation;
    using SFA.Infrastructure.Interfaces;
    using Validators;
    using ViewModels;

    public class ReportingMediator : MediatorBase, IReportingMediator
    {
        private readonly IReportingService _reportingService;
        private readonly ReportParametersDateRangeValidator _reportDateRangeValidator;
        private readonly ILogService _logService;

        public ReportingMediator(IReportingService reportingService, ILogService logService)
        {
            _reportingService = reportingService;
            _reportDateRangeValidator = new ReportParametersDateRangeValidator();
            _logService = logService;
        }

        public MediatorResponse<byte[]> GetVacanciesListReportBytes(ReportVacanciesParameters parameters)
        {
            try
            {
                var reportResult = _reportingService.ReportVacanciesList(parameters.FromDate.Date,
                    parameters.ToDate.Date);
                var bytes = GetCsvBytes(reportResult);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch(Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        public MediatorResponse<T> Validate<T>(T parameters) where T: ReportParameterBase
        {
            var validationResult = _reportDateRangeValidator.Validate(parameters);
            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.ValidationError, parameters, validationResult);
            }

            return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, parameters, validationResult);
        }

        public MediatorResponse<byte[]> GetSuccessfulCandidatesReportBytes(ReportSuccessfulCandidatesParameters parameters)
        {
            throw new NotImplementedException();
        }

        public MediatorResponse<byte[]> GetUnsuccessfulCandidatesReportBytes(
            ReportUnsuccessfulCandidatesParameters parameters)
        {
            try
            {
                var reportResult = _reportingService.ReportUnsuccessfulCandidates(parameters.Type,
                    parameters.FromDate.Date, parameters.ToDate.Date, parameters.AgeRange, parameters.ManagedBy, parameters.Region);
                var bytes = GetCsvBytes(reportResult);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        public MediatorResponse<ReportUnsuccessfulCandidatesParameters> GetUnsuccessfulCandidatesReportParams()
        {
            var result = new ReportUnsuccessfulCandidatesParameters();

            try
            {
                var localAuthorities = _reportingService.LocalAuthorityManagerGroups();
                result.ManagedByList = localAuthorities.ToListOfListItem();
                var regions = _reportingService.RegionsIncludingAll();
                result.RegionList = regions.ToListOfListItem();
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, result);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, result);
            }
        }

        public MediatorResponse<ReportSuccessfulCandidatesParameters> GetSuccessfulCandidatesReportParams()
        {
            var result = new ReportSuccessfulCandidatesParameters();

            try
            {
                var localAuthorities = _reportingService.LocalAuthorityManagerGroups();
                result.ManagedByList = localAuthorities.ToListOfListItem();
                var regions = _reportingService.RegionsIncludingAll();
                result.RegionList = regions.ToListOfListItem();
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, result);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, result);
            }
        }

        public MediatorResponse<byte[]> GetVacancyExtensionsReportBytes(ReportVacancyExtensionsParameters parameters)
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
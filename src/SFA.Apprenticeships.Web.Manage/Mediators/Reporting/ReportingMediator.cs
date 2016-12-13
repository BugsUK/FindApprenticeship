namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using Application.Interfaces;
    using Common.Constants;
    using Common.Extensions;
    using Common.Mediators;
    using Constants.Messages;
    using CsvClassMaps;
    using CsvHelper.Configuration;
    using Domain.Raa.Interfaces.Reporting;
    using Domain.Raa.Interfaces.Reporting.Models;
    using Infrastructure.Presentation;
    using Raa.Common.Validators.Report;
    using Raa.Common.ViewModels.Report;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using ViewModels;

    public class ReportingMediator : MediatorBase, IReportingMediator
    {
        private readonly IReportingRepository _reportingRepo;
        private readonly ReportParametersDateRangeValidator _reportDateRangeValidator;
        private readonly ILogService _logService;

        public ReportingMediator(IReportingRepository reportingRepo, ILogService logService)
        {
            _reportingRepo = reportingRepo;
            _reportDateRangeValidator = new ReportParametersDateRangeValidator();
            _logService = logService;
        }

        public MediatorResponse<byte[]> GetVacanciesListReportBytes(ReportVacanciesParameters parameters)
        {
            try
            {
                var reportResult = _reportingRepo.ReportVacanciesList(parameters.FromDate.Date,
                    parameters.ToDate.Date);
                var bytes = GetCsvBytes<ReportVacanciesResultItem, ReportVacanciesResultItemClassMap>(reportResult, "");
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        public MediatorResponse<T> Validate<T>(T parameters) where T : ReportParameterBase
        {
            var validationResult = _reportDateRangeValidator.Validate(parameters);
            parameters.IsValid = validationResult.IsValid;
            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.ValidationError, parameters, validationResult);
            }

            return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, parameters, validationResult);
        }

        public MediatorResponse<byte[]> GetSuccessfulCandidatesReportBytes(ReportSuccessfulCandidatesParameters parameters)
        {
            try
            {
                var reportResult = _reportingRepo.ReportSuccessfulCandidates(parameters.Type,
                    parameters.FromDate.Date, parameters.ToDate.Date, parameters.AgeRange, parameters.ManagedBy, parameters.Region);

                var headerBuilder = new StringBuilder();
                headerBuilder.AppendLine("PROTECT,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,");

                var bytes = parameters.IncludeCandidateIds
                    ? GetCsvBytes<ReportSuccessfulCandidatesResultItem, ReportSuccessfulCandidatesWithIdsResultItemClassMap>(
                        reportResult, headerBuilder.ToString())
                    : GetCsvBytes<ReportSuccessfulCandidatesResultItem, ReportSuccessfulCandidatesResultItemClassMap>(
                        reportResult, headerBuilder.ToString());
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        public MediatorResponse<byte[]> GetUnsuccessfulCandidatesReportBytes(
            ReportUnsuccessfulCandidatesParameters parameters)
        {
            try
            {
                var reportResult = _reportingRepo.ReportUnsuccessfulCandidates(parameters.Type,
                    parameters.FromDate.Date, parameters.ToDate.Date, parameters.AgeRange, parameters.ManagedBy, parameters.Region);

                var headerBuilder = new StringBuilder();
                headerBuilder.AppendLine("PROTECT,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine("Date,Total_Unsuccessful_Applications,Total_Candidates,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.Append(DateTime.Now.ToString("dd/MM/yyy")).Append(",");
                headerBuilder.Append(reportResult.Count).Append(",");
                headerBuilder.Append(reportResult.Select(i => i.CandidateId).Distinct().Count());
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");

                var bytes = parameters.IncludeCandidateIds
                    ? GetCsvBytes
                        <ReportUnsuccessfulCandidatesResultItem, ReportUnsuccessfulCandidatesWithIdsResultItemClassMap>(
                            reportResult, headerBuilder.ToString())
                    : GetCsvBytes
                        <ReportUnsuccessfulCandidatesResultItem, ReportUnsuccessfulCandidatesResultItemClassMap>(
                            reportResult, headerBuilder.ToString());

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
                var localAuthorities = _reportingRepo.LocalAuthorityManagerGroups();
                result.ManagedByList = localAuthorities.ToListItemList();
                var regions = _reportingRepo.GeoRegionsIncludingAll();
                result.RegionList = regions.ToListItemList();
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
                var localAuthorities = _reportingRepo.LocalAuthorityManagerGroups();
                result.ManagedByList = localAuthorities.ToListItemList();
                var regions = _reportingRepo.GeoRegionsIncludingAll();
                result.RegionList = regions.ToListItemList();
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
            try
            {
                int? vacancyStatus = null;

                if (parameters.Status != "All")
                {
                    vacancyStatus = int.Parse(parameters.Status);
                }

                var reportResult = _reportingRepo.ReportVacancyExtensions(parameters.FromDate.Date, parameters.ToDate.Date,
                    parameters.UKPRN, vacancyStatus);

                var bytes = GetCsvBytes<ReportVacancyExtensionsResultItem, ReportVacancyExtensionsResultItemClassMap>(reportResult, "");
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0], ReportingMessages.TimeoutMessage, UserMessageLevel.Warning);
            }
        }

        public MediatorResponse<ReportRegisteredCandidatesParameters> GetRegisteredCandidatesReportParams()
        {
            var result = new ReportRegisteredCandidatesParameters();

            try
            {
                var regions = _reportingRepo.GeoRegionsIncludingAll();
                result.RegionList = regions.ToListItemList();
                var localAuthorities = _reportingRepo.GetLocalAuthorities();
                result.LocalAuthoritiesList = localAuthorities.ToListItemList();
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, result);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, result);
            }
        }

        public MediatorResponse<ReportRegisteredCandidatesParameters> ValidateRegisteredCandidatesParameters(ReportRegisteredCandidatesParameters parameters)
        {
            var response = GetRegisteredCandidatesReportParams();
            if (response.Code == ReportingMediatorCodes.ReportCodes.Error)
                return response;

            parameters.RegionList = response.ViewModel.RegionList;
            parameters.LocalAuthoritiesList = response.ViewModel.LocalAuthoritiesList;

            var validationResult = _reportDateRangeValidator.Validate(parameters);
            parameters.IsValid = validationResult.IsValid;
            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.ValidationError, parameters, validationResult);
            }

            return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, parameters, validationResult);
        }

        public MediatorResponse<byte[]> GetRegisteredCandidatesReportBytes(ReportRegisteredCandidatesParameters parameters)
        {
            try
            {
                var reportResult = _reportingRepo.ReportRegisteredCandidates(parameters.Type, parameters.FromDate.Date, parameters.ToDate.Date, parameters.AgeRange, parameters.Region, parameters.LocalAuthority, parameters.MarketMessagesOnly);

                var headerBuilder = new StringBuilder();
                headerBuilder.AppendLine("PROTECT,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,");

                var bytes = parameters.IncludeCandidateIds
                    ? GetCsvBytes
                        <ReportRegisteredCandidatesResultItem, ReportRegisteredCandidatesWithIdsResultItemClassMap>(
                            reportResult, headerBuilder.ToString())
                    : GetCsvBytes
                        <ReportRegisteredCandidatesResultItem, ReportRegisteredCandidatesResultItemClassMap>(
                            reportResult, headerBuilder.ToString());
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportingMediatorCodes.ReportCodes.Error, new byte[0]);
            }
        }

        public MediatorResponse<ReportVacancyTrackerParameters> GetActionedVacancies()
        {
            throw new NotImplementedException();
        }

        private static byte[] GetCsvBytes<T, TClassMap>(IEnumerable<T> items, string header) where T : class where TClassMap : CsvClassMap<T>
        {
            var csvString = header + CsvPresenter.ToCsv<T, TClassMap>(items);
            var bytes = Encoding.UTF8.GetBytes(csvString);
            return bytes;
        }
    }
}
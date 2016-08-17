namespace SFA.Apprenticeships.Web.Recruit.Mediators.Report
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Apprenticeships.Application.Interfaces;
    using Common.Mediators;
    using CsvHelper.Configuration;
    using Domain.Entities.Raa.Reporting;
    using Infrastructure.Presentation;
    using Raa.Common.CsvClassMaps;
    using Raa.Common.Providers;
    using Raa.Common.Validators.Report;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Report;

    public class ReportMediator : MediatorBase, IReportMediator
    {
        private readonly ILogService _logService;
        private readonly IReportingProvider _reportingProvider;
        private readonly ReportParametersDateRangeValidator _reportDateRangeValidator;

        public ReportMediator(IReportingProvider reportingProvider, ILogService logService)
        {
            _logService = logService;
            _reportingProvider = reportingProvider;
            _reportDateRangeValidator = new ReportParametersDateRangeValidator();
        }

        public MediatorResponse<ApplicationsReceivedParameters> ValidateApplicationsReceivedParameters(ApplicationsReceivedParameters parameters)
        {
            var validationResult = _reportDateRangeValidator.Validate(parameters);
            parameters.IsValid = validationResult.IsValid;
            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ReportMediatorCodes.ValidateApplicationsReceivedParameters.ValidationError, parameters, validationResult);
            }

            return GetMediatorResponse(ReportMediatorCodes.ValidateApplicationsReceivedParameters.Ok, parameters, validationResult);
        }

        public MediatorResponse<byte[]> GetApplicationsReceived(ApplicationsReceivedParameters parameters, string username)
        {
            try
            {
                var reportResult = _reportingProvider.GetApplicationsReceivedResultItems(parameters.FromDate.Date, parameters.ToDate.Date, username);

                var headerBuilder = new StringBuilder();
                headerBuilder.AppendLine("PROTECT,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine("Date,Total_Number_Of_Applications,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.Append(DateTime.Now.ToString("dd/MM/yyy HH:mm")).Append(",");
                headerBuilder.Append(reportResult.Count);
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,");

                var bytes = GetCsvBytes<ApplicationsReceivedResultItem, ApplicationsReceivedResultItemClassMap>(reportResult, headerBuilder.ToString());
                return GetMediatorResponse(ReportMediatorCodes.GetApplicationsReceived.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportMediatorCodes.GetApplicationsReceived.ValidationError, new byte[0]);
            }
        }

        public MediatorResponse<CandidatesWithApplicationsParameters> ValidateCandidatesWithApplicationsParameters(CandidatesWithApplicationsParameters parameters)
        {
            var validationResult = _reportDateRangeValidator.Validate(parameters);
            parameters.IsValid = validationResult.IsValid;
            if (!validationResult.IsValid)
            {
                return GetMediatorResponse(ReportMediatorCodes.ValidateCandidatesWithApplicationsParameters.ValidationError, parameters, validationResult);
            }

            return GetMediatorResponse(ReportMediatorCodes.ValidateCandidatesWithApplicationsParameters.Ok, parameters, validationResult);
        }

        public MediatorResponse<byte[]> GetCandidatesWithApplications(CandidatesWithApplicationsParameters parameters, string username)
        {
            try
            {
                var reportResult = _reportingProvider.GetCandidatesWithApplicationsResultItem(parameters.FromDate.Date, parameters.ToDate.Date, username);

                var headerBuilder = new StringBuilder();
                headerBuilder.AppendLine("PROTECT,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                headerBuilder.AppendLine(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");

                var bytes = GetCsvBytes<CandidatesWithApplicationsResultItem, CandidatesWithApplicationsResultItemClassMap>(reportResult, headerBuilder.ToString());
                return GetMediatorResponse(ReportMediatorCodes.GetCandidatesWithApplications.Ok, bytes);
            }
            catch (Exception ex)
            {
                _logService.Warn(ex);
                return GetMediatorResponse(ReportMediatorCodes.GetCandidatesWithApplications.ValidationError, new byte[0]);
            }
        }

        private static byte[] GetCsvBytes<T, TClassMap>(IEnumerable<T> items, string header) where T : class where TClassMap : CsvClassMap<T>
        {
            var csvString = header + CsvPresenter.ToCsv<T, TClassMap>(items);
            var bytes = Encoding.UTF8.GetBytes(csvString);
            return bytes;
        }
    }
}
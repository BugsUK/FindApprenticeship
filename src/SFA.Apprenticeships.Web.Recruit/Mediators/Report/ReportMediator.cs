namespace SFA.Apprenticeships.Web.Recruit.Mediators.Report
{
    using Common.Mediators;
    using Raa.Common.Validators.Report;
    using SFA.Infrastructure.Interfaces;
    using ViewModels.Report;

    public class ReportMediator : MediatorBase, IReportMediator
    {
        private readonly ILogService _logService;
        private readonly ReportParametersDateRangeValidator _reportDateRangeValidator;

        public ReportMediator(ILogService logService)
        {
            _reportDateRangeValidator = new ReportParametersDateRangeValidator();
            _logService = logService;
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
    }
}
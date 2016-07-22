namespace SFA.Apprenticeships.Web.Recruit.Mediators.Report
{
    using Common.Mediators;
    using ViewModels.Report;

    public interface IReportMediator
    {
        MediatorResponse<ApplicationsReceivedParameters> ValidateApplicationsReceivedParameters(ApplicationsReceivedParameters parameters);
        MediatorResponse<CandidatesWithApplicationsParameters> ValidateCandidatesWithApplicationsParameters(CandidatesWithApplicationsParameters parameters);
    }
}
namespace SFA.Apprenticeships.Web.Recruit.Mediators.Report
{
    using Common.Mediators;
    using ViewModels.Report;

    public interface IReportMediator
    {
        MediatorResponse<ApplicationsReceivedParameters> ValidateApplicationsReceivedParameters(ApplicationsReceivedParameters parameters);
        MediatorResponse<byte[]> GetApplicationsReceived(ApplicationsReceivedParameters parameters, string username, string ukprn);
        MediatorResponse<CandidatesWithApplicationsParameters> ValidateCandidatesWithApplicationsParameters(CandidatesWithApplicationsParameters parameters);
        MediatorResponse<byte[]> GetCandidatesWithApplications(CandidatesWithApplicationsParameters parameters, string username, string ukprn);
    }
}
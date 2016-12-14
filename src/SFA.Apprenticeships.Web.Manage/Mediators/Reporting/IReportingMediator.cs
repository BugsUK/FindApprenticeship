namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using Common.Mediators;
    using Raa.Common.ViewModels.Report;
    using ViewModels;

    public interface IReportingMediator
    {
        MediatorResponse<T> Validate<T>(T parameters) where T : ReportParameterBase;
        MediatorResponse<byte[]> GetVacanciesListReportBytes(ReportVacanciesParameters parameters);
        MediatorResponse<byte[]> GetSuccessfulCandidatesReportBytes(ReportSuccessfulCandidatesParameters parameters);
        MediatorResponse<ReportSuccessfulCandidatesParameters> GetSuccessfulCandidatesReportParams();
        MediatorResponse<byte[]> GetUnsuccessfulCandidatesReportBytes(ReportUnsuccessfulCandidatesParameters parameters);
        MediatorResponse<ReportUnsuccessfulCandidatesParameters> GetUnsuccessfulCandidatesReportParams();
        MediatorResponse<byte[]> GetVacancyExtensionsReportBytes(ReportVacancyExtensionsParameters parameters);
        MediatorResponse<ReportRegisteredCandidatesParameters> GetRegisteredCandidatesReportParams();
        MediatorResponse<ReportRegisteredCandidatesParameters> ValidateRegisteredCandidatesParameters(ReportRegisteredCandidatesParameters parameters);
        MediatorResponse<byte[]> GetRegisteredCandidatesReportBytes(ReportRegisteredCandidatesParameters parameters);
        MediatorResponse<byte[]> GetVacancyTrackerReportBytes(ReportVacanciesParameters parameters);
    }
}

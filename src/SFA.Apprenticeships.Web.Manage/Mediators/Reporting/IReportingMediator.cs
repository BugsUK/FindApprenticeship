namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
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
        MediatorResponse<byte[]> GetRegisteredCandidatesReportBytes(ReportRegisteredCandidatesParameters parameters);
    }
}

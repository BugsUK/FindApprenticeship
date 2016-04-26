namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
    using ViewModels;

    public interface IReportingMediator
    {
        byte[] GetVacanciesListReportBytes(ReportVacanciesParameters parameters);
        byte[] GetSuccessfulCandidatesReportBytes(ReportSuccessfulCandidatesParameters parameters);
        byte[] GetUnsuccessfulCandidatesReportBytes(ReportUnsuccessfulCandidatesParameters parameters);
        ReportUnsuccessfulCandidatesParameters GetUnsuccessfulCandidatesReportParams();
        ReportSuccessfulCandidatesParameters GetSuccessfulCandidatesReportParams();
        byte[] GetVacancyExtensionsReportBytes(ReportVacancyExtensionsParameters parameters);
    }
}

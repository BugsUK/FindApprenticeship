namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;
    using ViewModels;

    public interface IReportingMediator
    {
        byte[] GetVacanciesListReportBytes(ReportVacanciesParameters parameters);
        byte[] GetSuccessfulCandidatesReportBytes(ReportSuccessfulCandidatesParameters parameters);
        byte[] GetUnsuccessfulCandidatesReportBytes(ReportUnsuccessfulCandidatesParameters parameters);
        byte[] GetVacancyExtensionsReportBytes(ReportVacancyExtensionsParameters parameters);
    }
}

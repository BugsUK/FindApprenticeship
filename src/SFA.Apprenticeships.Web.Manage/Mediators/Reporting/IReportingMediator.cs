namespace SFA.Apprenticeships.Web.Manage.Mediators.Reporting
{
    using System;

    public interface IReportingMediator
    {
        byte[] ReportVacanciesList(DateTime fromDate, DateTime toDate);
    }
}

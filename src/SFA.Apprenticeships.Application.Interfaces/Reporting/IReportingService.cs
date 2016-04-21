namespace SFA.Apprenticeships.Application.Interfaces.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Reporting.Models;

    public interface IReportingService
    {
        IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);
    }
}

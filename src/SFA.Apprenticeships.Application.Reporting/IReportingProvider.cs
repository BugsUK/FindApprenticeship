using System;
namespace SFA.Apprenticeships.Application.Reporting
{
    using System.Collections.Generic;
    using Domain.Raa.Interfaces.Reporting.Models;

    public interface IReportingProvider
    {
        IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);
    }
}

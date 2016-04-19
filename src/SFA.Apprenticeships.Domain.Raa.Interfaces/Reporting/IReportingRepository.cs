namespace SFA.Apprenticeships.Domain.Raa.Interfaces.Reporting
{
    using System;
    using System.Collections.Generic;
    using Models;

    public interface IReportingRepository
    {
        IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate);
    }
}

namespace SFA.Apprenticeships.Infrastructure.Raa
{
    using System;
    using System.Collections.Generic;
    using Application.Reporting;
    using Domain.Raa.Interfaces.Reporting.Models;

    public class ReportingProvider : IReportingProvider
    {
        public IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}

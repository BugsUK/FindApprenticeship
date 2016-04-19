namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Reporting
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Domain.Raa.Interfaces.Reporting;
    using Domain.Raa.Interfaces.Reporting.Models;
    using SFA.Infrastructure.Interfaces;

    public class ReportingRepository : IReportingRepository
    {
        private readonly ILogService _logger;
        private readonly IGetOpenConnection _getOpenConnection;

        public ReportingRepository(IGetOpenConnection getOpenConnection, ILogService logger)
        {
            _getOpenConnection = getOpenConnection;
            _logger = logger;
        }

        public IList<ReportVacanciesResultItem> ReportVacanciesList(DateTime fromDate, DateTime toDate)
        {
            throw new NotImplementedException();
        }
    }
}

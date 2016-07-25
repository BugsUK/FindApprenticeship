namespace SFA.Apprenticeships.Application.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;
    using Interfaces.Reporting;
    using Strategies;

    public class ReportingService : IReportingService
    {
        private readonly IGetApplicationsReceivedResultItemsStrategy _getApplicationsReceivedResultItemsStrategy;
        private readonly IGetCandidatesWithApplicationsResultItemsStrategy _getCandidatesWithApplicationsResultItemsStrategy;

        public ReportingService(IGetApplicationsReceivedResultItemsStrategy getApplicationsReceivedResultItemsStrategy, IGetCandidatesWithApplicationsResultItemsStrategy getCandidatesWithApplicationsResultItemsStrategy)
        {
            _getApplicationsReceivedResultItemsStrategy = getApplicationsReceivedResultItemsStrategy;
            _getCandidatesWithApplicationsResultItemsStrategy = getCandidatesWithApplicationsResultItemsStrategy;
        }

        public IEnumerable<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo)
        {
            return _getApplicationsReceivedResultItemsStrategy.Get(dateFrom, dateTo);
        }

        public IEnumerable<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItems(DateTime dateFrom, DateTime dateTo)
        {
            return _getCandidatesWithApplicationsResultItemsStrategy.Get(dateFrom, dateTo);
        }
    }
}
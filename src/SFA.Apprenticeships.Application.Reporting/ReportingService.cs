namespace SFA.Apprenticeships.Application.Reporting
{
    using Domain.Entities.Raa.Reporting;
    using Interfaces.Reporting;
    using Strategies;
    using System;
    using System.Collections.Generic;

    public class ReportingService : IReportingService
    {
        private readonly IGetApplicationsReceivedResultItemsStrategy _getApplicationsReceivedResultItemsStrategy;
        private readonly IGetCandidatesWithApplicationsResultItemsStrategy _getCandidatesWithApplicationsResultItemsStrategy;

        public ReportingService(IGetApplicationsReceivedResultItemsStrategy getApplicationsReceivedResultItemsStrategy, IGetCandidatesWithApplicationsResultItemsStrategy getCandidatesWithApplicationsResultItemsStrategy)
        {
            _getApplicationsReceivedResultItemsStrategy = getApplicationsReceivedResultItemsStrategy;
            _getCandidatesWithApplicationsResultItemsStrategy = getCandidatesWithApplicationsResultItemsStrategy;
        }

        public IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId)
        {
            return _getApplicationsReceivedResultItemsStrategy.Get(dateFrom, dateTo, providerSiteId);
        }

        public IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId)
        {
            return _getCandidatesWithApplicationsResultItemsStrategy.Get(dateFrom, dateTo, providerSiteId);
        }
    }
}
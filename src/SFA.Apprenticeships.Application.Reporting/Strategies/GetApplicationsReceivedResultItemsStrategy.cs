namespace SFA.Apprenticeships.Application.Reporting.Strategies
{
    using Domain.Entities.Raa.Reporting;
    using Domain.Raa.Interfaces.Reporting;
    using System;
    using System.Collections.Generic;

    public class GetApplicationsReceivedResultItemsStrategy : IGetApplicationsReceivedResultItemsStrategy
    {
        private readonly IReportingRepository _reportingRepository;


        public GetApplicationsReceivedResultItemsStrategy(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public IList<ApplicationsReceivedResultItem> Get(DateTime dateFrom, DateTime dateTo, int providerSiteId)
        {
            return _reportingRepository.GetApplicationsReceivedResultItems(dateFrom, dateTo, providerSiteId);
        }
    }
}
namespace SFA.Apprenticeships.Application.Reporting.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;
    using Domain.Raa.Interfaces.Reporting;

    public class GetApplicationsReceivedResultItemsStrategy : IGetApplicationsReceivedResultItemsStrategy
    {
        private readonly IReportingRepository _reportingRepository;

        public GetApplicationsReceivedResultItemsStrategy(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public IEnumerable<ApplicationsReceivedResultItem> Get(DateTime dateFrom, DateTime dateTo)
        {
            return _reportingRepository.GetApplicationsReceivedResultItems(dateFrom, dateTo);
        }
    }
}
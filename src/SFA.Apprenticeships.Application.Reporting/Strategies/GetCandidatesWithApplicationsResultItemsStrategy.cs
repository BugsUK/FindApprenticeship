namespace SFA.Apprenticeships.Application.Reporting.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;
    using Domain.Raa.Interfaces.Reporting;

    public class GetCandidatesWithApplicationsResultItemsStrategy : IGetCandidatesWithApplicationsResultItemsStrategy
    {
        private readonly IReportingRepository _reportingRepository;

        public GetCandidatesWithApplicationsResultItemsStrategy(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }

        public IEnumerable<CandidatesWithApplicationsResultItem> Get(DateTime dateFrom, DateTime dateTo)
        {
            return _reportingRepository.GetCandidatesWithApplicationsResultItems(dateFrom, dateTo);
        }
    }
}
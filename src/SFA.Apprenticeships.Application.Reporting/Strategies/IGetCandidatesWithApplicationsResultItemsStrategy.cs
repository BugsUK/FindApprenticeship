namespace SFA.Apprenticeships.Application.Reporting.Strategies
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;

    public interface IGetCandidatesWithApplicationsResultItemsStrategy
    {
        IEnumerable<CandidatesWithApplicationsResultItem> Get(DateTime dateFrom, DateTime dateTo);
    }
}
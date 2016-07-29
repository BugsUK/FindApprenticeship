namespace SFA.Apprenticeships.Application.Interfaces.Reporting
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;

    public interface IReportingService
    {
        IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId);
        IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItems(DateTime dateFrom, DateTime dateTo, int providerSiteId);
    }
}
 
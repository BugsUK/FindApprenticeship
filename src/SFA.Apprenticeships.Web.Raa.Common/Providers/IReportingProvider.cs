namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;

    public interface IReportingProvider
    {
        IEnumerable<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo);
        IEnumerable<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItem(DateTime dateFrom, DateTime dateTo);
    }
}
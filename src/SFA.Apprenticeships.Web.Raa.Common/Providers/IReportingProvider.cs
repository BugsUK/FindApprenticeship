namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Reporting;

    public interface IReportingProvider
    {
        IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, string username);
        IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItem(DateTime dateFrom, DateTime dateTo, string username);
    }
}
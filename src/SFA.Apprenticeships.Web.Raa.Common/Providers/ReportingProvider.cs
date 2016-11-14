namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Reporting;
    using Application.Interfaces.Users;
    using Domain.Entities.Raa.Reporting;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Providers;

    public class ReportingProvider : IReportingProvider
    {
        private readonly IReportingService _reportingService;
        private readonly IUserProfileService _userProfileService;
        private readonly IProviderService _providerService;

        public ReportingProvider(IReportingService reportingService, IUserProfileService userProfileService, IProviderService providerService)
        {
            _reportingService = reportingService;
            _userProfileService = userProfileService;
            _providerService = providerService;
        }

        public IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, string username, string ukprn)
        {
            return _reportingService.GetApplicationsReceivedResultItems(dateFrom, dateTo, GetProviderSiteId(username, ukprn));
        }

        public IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItem(DateTime dateFrom, DateTime dateTo, string username, string ukprn)
        {
            return _reportingService.GetCandidatesWithApplicationsResultItems(dateFrom, dateTo, GetProviderSiteId(username, ukprn));
        }

        private int GetProviderSiteId(string username, string ukprn)
        {
            var userProfile = _userProfileService.GetProviderUser(username);
            if (!userProfile.PreferredProviderSiteId.HasValue)
                throw new Exception($"User {username} does not have a specified PreferredProviderSiteId");

            var providerSites = _providerService.GetProviderSites(ukprn).ToList();
            var providerSiteId = userProfile.PreferredProviderSiteId.Value;
            if (providerSites.All(ps => ps.ProviderSiteId != providerSiteId))
            {
                providerSiteId = providerSites.First().ProviderSiteId;
            }

            return providerSiteId;
        }
    }
}
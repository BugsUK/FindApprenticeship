namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using Application.Interfaces.Reporting;
    using Application.Interfaces.Users;
    using Domain.Entities.Raa.Reporting;
    using System;
    using System.Collections.Generic;

    public class ReportingProvider : IReportingProvider
    {
        private readonly IReportingService _reportingService;
        private readonly IUserProfileService _userProfileService;


        public ReportingProvider(IReportingService reportingService, IUserProfileService userProfileService)
        {
            _reportingService = reportingService;
            _userProfileService = userProfileService;
        }

        public IList<ApplicationsReceivedResultItem> GetApplicationsReceivedResultItems(DateTime dateFrom, DateTime dateTo, string username)
        {
            var userProfile = _userProfileService.GetProviderUser(username);
            if (!userProfile.PreferredProviderSiteId.HasValue)
                throw new Exception($"User {username} does not have a specified PreferredProviderSiteId");

            return _reportingService.GetApplicationsReceivedResultItems(dateFrom, dateTo, userProfile.PreferredProviderSiteId.Value);
        }

        public IList<CandidatesWithApplicationsResultItem> GetCandidatesWithApplicationsResultItem(DateTime dateFrom, DateTime dateTo, string username)
        {
            var userProfile = _userProfileService.GetProviderUser(username);
            if (!userProfile.PreferredProviderSiteId.HasValue)
                throw new Exception($"User {username} does not have a specified PreferredProviderSiteId");

            return _reportingService.GetCandidatesWithApplicationsResultItems(dateFrom, dateTo, userProfile.PreferredProviderSiteId.Value);
        }
    }
}
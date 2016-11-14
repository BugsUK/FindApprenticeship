namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;
    using System.Collections.Generic;
    using Domain.Entities.Raa.Users;
    using Domain.Raa.Interfaces.Repositories.Models;

    /// <summary>
    /// For managing user profiles for users that are authenticated outside the service (ie. SSO). 
    /// Uses the user profiles repository
    /// </summary>
    public interface IUserProfileService
    {
        ProviderUser GetProviderUser(int providerUserId);

        ProviderUser GetProviderUser(string username);

        IEnumerable<ProviderUser> GetProviderUsers(string ukprn);

        IEnumerable<ProviderUser> SearchProviderUsers(ProviderUserSearchParameters searchParameters);

        ProviderUser CreateProviderUser(ProviderUser providerUser);

        ProviderUser UpdateProviderUser(ProviderUser providerUser);

        AgencyUser GetAgencyUser(string username);

        AgencyUser SaveUser(AgencyUser agencyUser);

        IEnumerable<Role> GetRoles();
    }
}

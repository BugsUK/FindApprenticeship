namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System.Collections.Generic;
    using Domain.Entities.Users;

    /// <summary>
    /// For managing user profiles for users that are authenticated outside the service (ie. SSO). 
    /// Uses the user profiles repository
    /// </summary>
    public interface IUserProfileService
    {
        ProviderUser GetProviderUser(string username);

        IEnumerable<ProviderUser> GetProviderUsers(string ukprn);

        ProviderUser SaveUser(ProviderUser providerUser);

        AgencyUser GetAgencyUser(string username);

        AgencyUser SaveUser(AgencyUser agencyUser);

        IEnumerable<Team> GetTeams();

        IEnumerable<Role> GetRoles(string roleList);
    }
}

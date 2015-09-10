namespace SFA.Apprenticeships.Application.Interfaces.Users
{
    using System;

    /// <summary>
    /// For managing user profiles for users that are authenticated outside the service (ie. SSO). 
    /// Uses the user profiles repository
    /// </summary>
    public interface IUserProfileService
    {
        // GetProviderUser (id) -> ProviderUser
        // SaveUser (ProviderUser)
    }
}

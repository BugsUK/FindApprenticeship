namespace SFA.Apprenticeships.Application.UserProfile
{
    using Domain.Entities.Users;

    public interface IProviderUserService
    {
        ProviderUser GetProviderUser(string username);
        ProviderUser CreateProviderUser(ProviderUser providerUser);
        ProviderUser UpdateProviderUser(ProviderUser providerUser);
    }
}
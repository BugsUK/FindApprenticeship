namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using Application.Interfaces.Users;
    using Domain.Entities.Users;
    using ViewModels;

    public class AgencyUserProvider : IAgencyUserProvider
    {
        private readonly IUserProfileService _userProfileService;

        public AgencyUserProvider(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        public AgencyUserViewModel GetOrCreateAgencyUser(string username)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            if (agencyUser == null)
            {
                agencyUser = new AgencyUser
                {
                    Username = username
                };
                agencyUser = _userProfileService.SaveUser(agencyUser);
            }

            return new AgencyUserViewModel
            {
                
            };
        }
    }
}
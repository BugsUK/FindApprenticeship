namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System.Linq;
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

        public AgencyUserViewModel GetOrCreateAgencyUser(string username, string roleList)
        {
            var teams = _userProfileService.GetTeams().ToList();
            var roles = _userProfileService.GetRoles(roleList).ToList();

            var agencyUser = _userProfileService.GetAgencyUser(username);
            if (agencyUser == null)
            {
                agencyUser = new AgencyUser
                {
                    Username = username,
                    Team = teams.Single(t => t.IsDefault),
                    Role = roles.Single(r => r.IsDefault)
                };
                agencyUser = _userProfileService.SaveUser(agencyUser);
            }

            return new AgencyUserViewModel
            {
                TeamId = agencyUser.Team.Id,
                RoleId = agencyUser.Role.Id
            };
        }
    }
}
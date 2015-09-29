namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
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
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var teams = _userProfileService.GetTeams().ToList();
            var roles = _userProfileService.GetRoles(roleList).ToList();

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

            return GetAgencyUserViewModel(agencyUser, teams, roles);
        }

        public AgencyUserViewModel GetAgencyUser(string username, string roleList)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var teams = _userProfileService.GetTeams().ToList();
            var roles = _userProfileService.GetRoles(roleList).ToList();

            return GetAgencyUserViewModel(agencyUser, teams, roles);
        }

        public AgencyUserViewModel SaveAgencyUser(string username, string roleList, AgencyUserViewModel viewModel)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var teams = _userProfileService.GetTeams().ToList();
            var roles = _userProfileService.GetRoles(roleList).ToList();

            var team = teams.Single(t => t.Id == viewModel.TeamId);
            var role = roles.Single(r => r.Id == viewModel.RoleId);
            agencyUser.Team = team;
            agencyUser.Role = role;

            _userProfileService.SaveUser(agencyUser);

            return GetAgencyUserViewModel(agencyUser, teams, roles);
        }

        private static List<SelectListItem> GetTeamsSelectList(IEnumerable<Team> teams, string teamId)
        {
            var teamsSelectList = teams.Select(t => new SelectListItem { Value = t.Id, Text = t.Name, Selected = t.Id == teamId }).ToList();

            return teamsSelectList;
        }

        private static List<SelectListItem> GetRolesSelectList(IEnumerable<Role> roles, string roleId)
        {
            var rolesSelectList = roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name, Selected = r.Id == roleId }).ToList();

            return rolesSelectList;
        }

        private static AgencyUserViewModel GetAgencyUserViewModel(AgencyUser agencyUser, IEnumerable<Team> teams, IEnumerable<Role> roles)
        {
            return new AgencyUserViewModel
            {
                Teams = GetTeamsSelectList(teams, agencyUser.Team.Id),
                TeamId = agencyUser.Team.Id,
                Roles = GetRolesSelectList(roles, agencyUser.Role.Id),
                RoleId = agencyUser.Role.Id
            };
        }
    }
}
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
                    Username = username
                };
                agencyUser = _userProfileService.SaveUser(agencyUser);
            }

            if (agencyUser.Role == null)
            {
                agencyUser.Role = roles.Single(r => r.IsDefault);
            }

            if (agencyUser.Team == null)
            {
                agencyUser.Team = teams.Single(r => r.IsDefault);
            }

            return GetAgencyUserViewModel(agencyUser, teams, roles);
        }

        public AgencyUserViewModel GetAgencyUser(string username, string roleList)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var teams = _userProfileService.GetTeams().ToList();
            var roles = _userProfileService.GetRoles(roleList).ToList();

            if (agencyUser.Role == null)
            {
                agencyUser.Role = roles.Single(r => r.IsDefault);
            }

            if (agencyUser.Team == null)
            {
                agencyUser.Team = teams.Single(r => r.IsDefault);
            }

            return GetAgencyUserViewModel(agencyUser, teams, roles);
        }

        public AgencyUserViewModel SaveAgencyUser(string username, string roleList, AgencyUserViewModel viewModel)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var teams = _userProfileService.GetTeams().ToList();
            var roles = _userProfileService.GetRoles(roleList).ToList();

            var team = teams.Single(t => t.CodeName == viewModel.TeamCode);
            var role = roles.Single(r => r.CodeName == viewModel.RoleCode);
            agencyUser.Team = team;
            agencyUser.Role = role;

            _userProfileService.SaveUser(agencyUser);

            return GetAgencyUserViewModel(agencyUser, teams, roles);
        }

        private static List<SelectListItem> GetTeamsSelectList(IEnumerable<Team> teams, string teamId)
        {
            var teamsSelectList = teams.Select(t => new SelectListItem { Value = t.CodeName, Text = t.Name, Selected = t.CodeName == teamId }).ToList();

            return teamsSelectList;
        }

        private static List<SelectListItem> GetRolesSelectList(IEnumerable<Role> roles, string roleId)
        {
            var rolesSelectList = roles.Select(r => new SelectListItem { Value = r.CodeName, Text = r.Name, Selected = r.CodeName == roleId }).ToList();

            return rolesSelectList;
        }

        private static AgencyUserViewModel GetAgencyUserViewModel(AgencyUser agencyUser, IEnumerable<Team> teams, IEnumerable<Role> roles)
        {
            return new AgencyUserViewModel
            {
                Teams = GetTeamsSelectList(teams, agencyUser.Team.CodeName),
                TeamCode = agencyUser.Team.CodeName,
                Roles = GetRolesSelectList(roles, agencyUser.Role.CodeName),
                RoleCode = agencyUser.Role.CodeName
            };
        }
    }
}
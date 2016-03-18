namespace SFA.Apprenticeships.Web.Manage.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Application.Interfaces.Users;
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Users;
    using Infrastructure.Presentation;
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
            var roles = _userProfileService.GetRoles(roleList).ToList();

            if (agencyUser == null)
            {
                agencyUser = new AgencyUser
                {
                    Username = username,
                    Role = roles.Single(r => r.IsDefault),
                    RegionalTeam = RegionalTeam.North
                };
                agencyUser = _userProfileService.SaveUser(agencyUser);
            }

            return GetAgencyUserViewModel(agencyUser, roles);
        }

        public AgencyUserViewModel GetAgencyUser(string username, string roleList)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var roles = _userProfileService.GetRoles(roleList).ToList();

            return GetAgencyUserViewModel(agencyUser, roles);
        }

        public AgencyUserViewModel SaveAgencyUser(string username, string roleList, AgencyUserViewModel viewModel)
        {
            var agencyUser = _userProfileService.GetAgencyUser(username);
            var roles = _userProfileService.GetRoles(roleList).ToList();

            if (!string.IsNullOrEmpty(viewModel.RoleId))
            {
                var role = roles.Single(r => r.Id == viewModel.RoleId);
                agencyUser.Role = role;
            }
            agencyUser.RegionalTeam = viewModel.RegionalTeam;

            _userProfileService.SaveUser(agencyUser);

            return GetAgencyUserViewModel(agencyUser, roles);
        }

        private static List<SelectListItem> GetRolesSelectList(IEnumerable<Role> roles, string roleId)
        {
            var rolesSelectList = roles.Select(r => new SelectListItem { Value = r.Id, Text = r.Name, Selected = r.Id == roleId }).ToList();

            return rolesSelectList;
        }

        private static List<SelectListItem> GetRegionalTeamsSelectList(RegionalTeam regionalTeam)
        {
            var regionalTeams =
                Enum.GetValues(typeof(RegionalTeam))
                    .Cast<RegionalTeam>()
                    .Where(rt => rt != RegionalTeam.Other)
                    .Select(rt => new SelectListItem { Value = rt.ToString(), Text = rt.GetTitle(), Selected = rt == regionalTeam })
                    .ToList();

            return regionalTeams;
        }

        private static AgencyUserViewModel GetAgencyUserViewModel(AgencyUser agencyUser, IEnumerable<Role> roles)
        {
            if (agencyUser.Role == null)
            {
                agencyUser.Role = roles.First(r => r.IsDefault);
            }

            return new AgencyUserViewModel
            {
                Roles = GetRolesSelectList(roles, agencyUser.Role.Id),
                RoleId = agencyUser.Role.Id,
                RegionalTeams = GetRegionalTeamsSelectList(agencyUser.RegionalTeam),
                RegionalTeam = agencyUser.RegionalTeam
            };
        }
    }
}
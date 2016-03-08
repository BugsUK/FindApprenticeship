namespace SFA.Apprenticeships.Web.Manage.UnitTests.Factory
{
    using System.Collections.Generic;
    using Domain.Entities.Raa.Users;

    public class RoleListFactory
    {
        public static IEnumerable<Role> GetRoleList(string roleList)
        {
            return new List<Role>
            {
                GetRole("Helpdesk_advisor", "Helpdesk advisor"),
                GetRole("QA_advisor", "QA advisor", true),
                GetRole("Technical_advisor", "Technical advisor")
            };
        }

        public static Role GetRole(string id, string name, bool isDefault = false)
        {
            return new Role
            {
                Id = id,
                Name = name,
                IsDefault = isDefault
            };
        }
    }
}
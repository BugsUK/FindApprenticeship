namespace SFA.Apprenticeships.Web.Manage.UnitTests.Factory
{
    using System.Collections.Generic;
    using Domain.Entities.Users;

    public class RoleListFactory
    {
        public static IEnumerable<Role> GetRoleList(string roleList)
        {
            return new List<Role>
            {
                GetRole(Role.CodeNameHelpdeskAdviser, "Helpdesk adviser"),
                GetRole(Role.CodeNameQAAdviser, "QA adviser", true),
                GetRole(Role.CodeNameTechnicalAdviser, "Technical adviser")
            };
        }

        public static Role GetRole(string codeName, string name, bool isDefault = false)
        {
            return new Role
            {
                CodeName = codeName,
                Name = name,
                IsDefault = isDefault
            };
        }
    }
}
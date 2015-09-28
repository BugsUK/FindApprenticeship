namespace SFA.Apprenticeships.Web.Manage.UnitTests.Builders
{
    using Domain.Entities.Users;
    using Factory;

    public class AgencyUserBuilder
    {
        private readonly string _username;

        public AgencyUserBuilder(string username)
        {
            _username = username;
        }

        public AgencyUser Build()
        {
            return new AgencyUser
            {
                Username = _username,
                Team = TeamListFactory.GetTeam("All", "All", true),
                Role = RoleListFactory.GetRole("QA_advisor", "QA advisor", true)
            };
        }
    }
}
namespace SFA.Apprenticeships.Web.Manage.UnitTests.Builders
{
    using Domain.Entities.Raa.Users;
    using Factory;

    public class AgencyUserBuilder
    {
        private readonly string _username;
        private Team _team;
        private Role _role;

        public AgencyUserBuilder(string username)
        {
            _username = username;
            _team = TeamListFactory.GetTeam("All", "All", true);
            _role = RoleListFactory.GetRole("QA_advisor", "QA advisor", true);
        }

        public AgencyUser Build()
        {
            return new AgencyUser
            {
                Username = _username,
                Team = _team,
                Role = _role
            };
        }

        public AgencyUserBuilder WithTeam(Team team)
        {
            _team = team;
            return this;
        }

        public AgencyUserBuilder WithRole(Role role)
        {
            _role = role;
            return this;
        }
    }
}
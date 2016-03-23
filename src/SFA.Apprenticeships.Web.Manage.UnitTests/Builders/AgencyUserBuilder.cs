namespace SFA.Apprenticeships.Web.Manage.UnitTests.Builders
{
    using Domain.Entities.Raa.Reference;
    using Domain.Entities.Raa.Users;
    using Factory;

    public class AgencyUserBuilder
    {
        private readonly string _username;
        private Role _role;
        private RegionalTeam _regionalTeam;

        public AgencyUserBuilder(string username)
        {
            _username = username;
            _role = RoleListFactory.GetRole("QA_advisor", "QA advisor", true);
            _regionalTeam = RegionalTeam.North;
        }

        public AgencyUser Build()
        {
            return new AgencyUser
            {
                Username = _username,
                Role = _role,
                RegionalTeam = _regionalTeam
            };
        }

        public AgencyUserBuilder WithRegionalTeam(RegionalTeam regionalTeam)
        {
            _regionalTeam = regionalTeam;
            return this;
        }

        public AgencyUserBuilder WithRole(Role role)
        {
            _role = role;
            return this;
        }
    }
}
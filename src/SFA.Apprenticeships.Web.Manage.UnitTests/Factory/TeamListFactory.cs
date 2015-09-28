namespace SFA.Apprenticeships.Web.Manage.UnitTests.Factory
{
    using System.Collections.Generic;
    using Domain.Entities.Users;

    public class TeamListFactory
    {
        public static IEnumerable<Team> GetTeams()
        {
            return new List<Team>
            {
                GetTeam("All", "All", true)
            };
        }

        public static Team GetTeam(string id, string name, bool isDefault = false)
        {
            return new Team
            {
                Id = id,
                Name = name,
                IsDefault = isDefault
            };
        }
    }
}
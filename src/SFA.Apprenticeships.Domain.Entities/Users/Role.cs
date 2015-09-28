namespace SFA.Apprenticeships.Domain.Entities.Users
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        //TODO: Find out from Mark what this is for
        public bool AllowTeamSelection { get; set; }
        public bool IsDefault { get; set; }
    }
}
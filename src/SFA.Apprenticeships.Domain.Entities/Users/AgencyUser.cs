namespace SFA.Apprenticeships.Domain.Entities.Users
{
    public class AgencyUser : BaseEntity
    {
        public string Username { get; set; }
        public Team Team { get; set; }
        public Role Role { get; set; }
    }
}
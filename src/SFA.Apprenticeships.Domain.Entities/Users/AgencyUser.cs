namespace SFA.Apprenticeships.Domain.Entities.Users
{
    using System;

    public class AgencyUser : BaseEntity<Guid>
    {
        public string Username { get; set; }
        public Team Team { get; set; }
        public Role Role { get; set; }
    }
}
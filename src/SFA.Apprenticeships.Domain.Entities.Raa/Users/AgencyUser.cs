namespace SFA.Apprenticeships.Domain.Entities.Raa.Users
{
    using System;
    using Reference;

    public class AgencyUser : ICreatableEntity, IUpdatableEntity
    {
        public int AgencyUserId { get; set; }
        public Guid AgencyUserGuid { get; set; }
        public string Username { get; set; }
        public Role Role { get; set; }
        public RegionalTeam RegionalTeam { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}
namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProfile.AgencyUser")]
    public class AgencyUser
    {
        public int AgencyUserId { get; set; }

        public Guid AgencyUserGuid { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime? UpdatedDateTime { get; set; }

        public string Username { get; set; }
    }
}

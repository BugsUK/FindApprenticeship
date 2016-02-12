namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProfile.AgencyUser")]
    public class AgencyUser
    {
        public int AgencyUserId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DateUpdated { get; set; }

        public string Username { get; set; }
    }
}

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile.Entities
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProfile.Team")]
    public class Team
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDefault { get; set; }
    }
}

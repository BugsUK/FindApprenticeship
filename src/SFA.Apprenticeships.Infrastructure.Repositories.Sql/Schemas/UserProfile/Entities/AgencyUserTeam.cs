namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.UserProfile.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("UserProfile.AgencyUserTeam")]
    public class AgencyUserTeam
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AgencyUserTeamId { get; set; }

        [Required]
        [StringLength(3)]
        public string CodeName { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int IsDefault { get; set; }
    }
}

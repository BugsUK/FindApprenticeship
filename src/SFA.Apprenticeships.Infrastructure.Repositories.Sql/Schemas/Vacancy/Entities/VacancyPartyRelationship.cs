namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Vacancy.VacancyPartyRelationship")]
    public class VacancyPartyRelationship
    {
        public int VacancyPartyRelationshipId { get; set; }

        public int FromVacancyPartyId { get; set; }

        public int ToVacancyPartyId { get; set; }

        [StringLength(3)]
        public string VacancyPartyRelationshipTypeCode { get; set; }

        public string DescriptionOfToPartyUsedByFromParty { get; set; }
        public string WebsiteUrlOfToPartyUsedByFromParty { get; set; }
    }
}

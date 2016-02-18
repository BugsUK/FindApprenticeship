namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    using Dapper.Contrib.Extensions;

    [Table("dbo.VacancyOwnerRelationship")]
    public class VacancyOwnerRelationship
    {
        //TODO: SQL: Created and Updated Date

        [Key]
        public int VacancyOwnerRelationshipId { get; set; }

        public int EmployerId { get; set; }

        public int ProviderSiteID { get; set; }

        public int ContractHolderIsEmployer { get; set; }

        public int ManagerIsEmployer { get; set; }

        public int StatusTypeId { get; set; }

        public string Notes { get; set; }

        public string EmployerDescription { get; set; }

        public string EmployerWebsite { get; set; }

        public int? EmployerLogoAttachmentId { get; set; }

        public int NationwideAllowed { get; set; }
    }
}

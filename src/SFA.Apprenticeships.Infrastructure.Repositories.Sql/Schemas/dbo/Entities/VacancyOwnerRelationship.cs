namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo.Entities
{
    public class VacancyOwnerRelationship
    {
        public int VacancyOwnerRelationshipId { get; set; }
	    public int EmployerId { get; set; }
        public int ProviderSiteID { get; set; }
        public bool ContractHolderIsEmployer { get; set; }
        public bool ManagerIsEmployer { get; set; }
        public int StatusTypeId { get; set; }
        public string Notes { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsite { get; set; }
        public int? EmployerLogoAttachmentId { get; set; }
        public bool NationWideAllowed { get; set; }
        public bool EditedInRaa { get; set; }
    }
}
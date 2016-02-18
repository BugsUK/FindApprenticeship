namespace SFA.Apprenticeships.Infrastructure.TacticalDataServices.Models
{
    public class VacancyOwnerRelationship
    {
        public int VacancyOwnerRelationshipId { get; set; }
        public int ProviderSiteId { get; set; }
        public int ProviderSiteEdsUrn { get; set; }
        public bool ContractHolderIsEmployer { get; set; }
        public bool ManagerIsEmployer { get; set; }
        public int StatusTypeId { get; set; }
        public string Notes { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsite { get; set; }
        public bool NationWideAllowed { get; set; }
        public Employer Employer { get; set; }
    }
}
namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;

    public class VacancyOwnerRelationship
    {
        public int VacancyOwnerRelationshipId { get; set; }
        public Guid VacancyOwnerRelationshipGuid { get; set; }
        public int ProviderSiteId { get; set; }
        public int EmployerId { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }

    }
}

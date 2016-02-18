namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;

    public class VacancyParty : ICreatableEntity, IUpdatableEntity
    {
        public int VacancyPartyId { get; set; }
        public int ProviderSiteId { get; set; }
        public int EmployerId { get; set; }
        public string EmployerDescription { get; set; }
        public string EmployerWebsiteUrl { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}

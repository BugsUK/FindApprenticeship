namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using System;
    using Locations;

    public class ProviderSite : ICreatableEntity, IUpdatableEntity
    {
        public int ProviderSiteId { get; set; }
        public Guid ProviderSiteGuid { get; set; }
        public string EdsErn { get; set; }
        public int ProviderId { get; set; }
        public string Ukprn { get; set; }
        public string Name { get; set; }
        public string EmployerDescription { get; set; }
        public string CandidateDescription { get; set; }
        public string ContactDetailsForEmployer { get; set; }
        public string ContactDetailsForCandidate { get; set; }
        public PostalAddress Address { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime? UpdatedDateTime { get; set; }
    }
}

namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using Locations;

    public class ProviderSite
    {
        public int ProviderSiteId { get; set; }

        public string EdsUrn { get; set; }

        public int ProviderId { get; set; }

        public string Name { get; set; }

        public string TradingName { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public PostalAddress Address { get; set; }
    }
}

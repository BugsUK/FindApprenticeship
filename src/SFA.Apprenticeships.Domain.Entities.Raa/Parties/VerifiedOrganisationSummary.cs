namespace SFA.Apprenticeships.Domain.Entities.Raa.Parties
{
    using Locations;

    public class VerifiedOrganisationSummary
    {
        public string ReferenceNumber { get; set; }

        public string FullName { get; set; }

        public string TradingName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string WebSite { get; set; }

        public PostalAddress Address { get; set; }
    }
}

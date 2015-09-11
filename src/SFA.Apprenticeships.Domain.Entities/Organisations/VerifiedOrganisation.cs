namespace SFA.Apprenticeships.Domain.Entities.Organisations
{
    using Locations;

    public class VerifiedOrganisation
    {
        public string ReferenceNumber { get; set; }

        public string Name { get; set; }

        public string TradingName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public Address Address { get; set; }
    }
}

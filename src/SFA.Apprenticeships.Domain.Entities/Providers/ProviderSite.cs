namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    public class ProviderSite : BaseEntity
    {
        // name, address, ERN, contact info, etc.

        public string Ern { get; set; }

        public string Ukprn { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }
    }
}

namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    using Locations;

    public class ProviderSite : BaseEntity
    {
        public string Ern { get; set; }

        public string Ukprn { get; set; }

        public string Name { get; set; }

        public string EmployerDescription { get; set; }

        public string CandidateDescription { get; set; }

        public string ContactDetailsForEmployer { get; set; }

        public string ContactDetailsForCandidate { get; set; }

        public Address Address { get; set; }
    }
}

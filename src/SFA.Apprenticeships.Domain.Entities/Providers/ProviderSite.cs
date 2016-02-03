namespace SFA.Apprenticeships.Domain.Entities.Providers
{
    using System;
    using Locations;

    public class ProviderSite : BaseEntity<Guid>
    {
        //TODO: Review the name of this property - It's specific to employers (Employer Reference Number) and should perhaps be EDSURN (Employer Data Service Unique Reference Number)
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

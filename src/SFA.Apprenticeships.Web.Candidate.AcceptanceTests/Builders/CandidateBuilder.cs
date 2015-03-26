namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using Common.Configuration;
    using Domain.Entities.Candidates;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Repositories;
    using IoC;
    
    public class CandidateBuilder
    {
        public CandidateBuilder(string emailAddress)
        {
            RegistrationBuilder = new RegistrationBuilder(emailAddress);

            Candidate = new Candidate
            {
                EntityId = UserBuilder.UserAndCandidateId,
                DateCreated = DateTime.Now
            };
        }

        public Candidate Candidate { get; private set; }

        public RegistrationBuilder RegistrationBuilder { get; private set; }

        public Candidate Build()
        {
            var repo = WebTestRegistry.Container.GetInstance<ICandidateWriteRepository>();
            var repoRead = WebTestRegistry.Container.GetInstance<ICandidateReadRepository>();
            var configuration = WebTestRegistry.Container.GetInstance<IConfigurationService>();

            Candidate.RegistrationDetails = RegistrationBuilder.Build();
            Candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = configuration.Get<WebConfiguration>().TermsAndConditionsVersion;
            Candidate.CommunicationPreferences = new CommunicationPreferences();

            var candidateInRepo = repoRead.Get(Candidate.RegistrationDetails.EmailAddress);

            if (candidateInRepo != null)
            {
                Candidate.EntityId = candidateInRepo.EntityId;
            }

            repo.Save(Candidate);

            return Candidate;
        }
    }
}

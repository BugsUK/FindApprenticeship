namespace SFA.Apprenticeships.Web.Candidate.AcceptanceTests.Builders
{
    using System;
    using Common.Configuration;
    using Domain.Entities.Candidates;
    using SFA.Infrastructure.Interfaces;
    using Domain.Interfaces.Repositories;
    using IoC;

    using SFA.Apprenticeships.Application.Interfaces;

    public class CandidateBuilder
    {
        public CandidateBuilder(string emailAddress)
        {
            RegistrationBuilder = new RegistrationBuilder(emailAddress);

            Candidate = new Candidate
            {
                EntityId = UserBuilder.UserAndCandidateId,
                DateCreated = DateTime.UtcNow
            };
        }

        public Candidate Candidate { get; private set; }

        public RegistrationBuilder RegistrationBuilder { get; private set; }

        public Candidate Build()
        {
            var repo = WebTestRegistry.Container.GetInstance<ICandidateWriteRepository>();
            var repoRead = WebTestRegistry.Container.GetInstance<IUserReadRepository>();
            var configuration = WebTestRegistry.Container.GetInstance<IConfigurationService>();

            Candidate.RegistrationDetails = RegistrationBuilder.Build();
            Candidate.RegistrationDetails.AcceptedTermsAndConditionsVersion = configuration.Get<CommonWebConfiguration>().TermsAndConditionsVersion;
            Candidate.CommunicationPreferences = new CommunicationPreferences();

            var userInRepo = repoRead.Get(Candidate.RegistrationDetails.EmailAddress);

            if (userInRepo != null)
            {
                Candidate.EntityId = userInRepo.EntityId;
            }

            repo.Save(Candidate);

            return Candidate;
        }
    }
}

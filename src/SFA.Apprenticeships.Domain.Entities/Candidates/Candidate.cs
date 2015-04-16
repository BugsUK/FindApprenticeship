namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using Users;

    public class Candidate : BaseEntity
    {
        public Candidate()
        {
            RegistrationDetails = new RegistrationDetails();
            ApplicationTemplate = new ApplicationTemplate();
            CommunicationPreferences = new CommunicationPreferences();
            HelpPreferences = new HelpPreferences();
        }

        public int LegacyCandidateId { get; set; } // temporary "weak link" to legacy candidate record (could be via an index)

        public RegistrationDetails RegistrationDetails { get; set; }

        public ApplicationTemplate ApplicationTemplate { get; set; }

        public CommunicationPreferences CommunicationPreferences { get; set; }

        public HelpPreferences HelpPreferences { get; set; }
    }
}

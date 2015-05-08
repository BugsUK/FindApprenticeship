﻿namespace SFA.Apprenticeships.Domain.Entities.Candidates
{
    using System;
    using Users;

    public class Candidate : BaseEntity
    {
        public Candidate()
        {
            RegistrationDetails = new RegistrationDetails();
            ApplicationTemplate = new ApplicationTemplate();
            CommunicationPreferences = new CommunicationPreferences();
            HelpPreferences = new HelpPreferences();
            MonitoringInformation = new MonitoringInformation();
            SubscriberId = Guid.NewGuid();
        }

        public int LegacyCandidateId { get; set; } // temporary "weak link" to legacy candidate record (could be via an index)

        public Guid SubscriberId { get; set; }

        public RegistrationDetails RegistrationDetails { get; set; }

        public ApplicationTemplate ApplicationTemplate { get; set; }

        public CommunicationPreferences CommunicationPreferences { get; set; }

        public HelpPreferences HelpPreferences { get; set; }

        public MonitoringInformation MonitoringInformation { get; set; }
    }
}

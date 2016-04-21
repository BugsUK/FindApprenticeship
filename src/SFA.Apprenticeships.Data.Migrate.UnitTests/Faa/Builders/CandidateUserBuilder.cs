namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Builders
{
    using System;
    using Migrate.Faa.Entities.Mongo;

    public class CandidateUserBuilder
    {
        private Guid _candidateId = Guid.NewGuid();
        private int _legacyCandidateId = 456789;
        private int _status = 20;

        public CandidateUser Build()
        {
            var candidate = new Candidate
            {
                Id = _candidateId,
                DateCreated = DateTime.Now.AddDays(-7),
                DateUpdated = DateTime.Now,
                LegacyCandidateId = _legacyCandidateId
            };

            var user = new User
            {
                Id = _candidateId,
                Status = _status
            };

            return new CandidateUser
            {
                Candidate = candidate,
                User = user
            };
        }

        public CandidateUserBuilder WithCandidateId(Guid candidateId)
        {
            _candidateId = candidateId;
            return this;
        }

        public CandidateUserBuilder WithLegacyCandidateId(int legacyCandidateId)
        {
            _legacyCandidateId = legacyCandidateId;
            return this;
        }

        public CandidateUserBuilder WithStatus(int status)
        {
            _status = status;
            return this;
        }
    }
}
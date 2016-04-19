namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Builders
{
    using System;
    using Migrate.Faa.Entities.Mongo;

    public class CandidateBuilder
    {
        private Guid _candidateId = Guid.NewGuid();
        private int _legacyCandidateId = 456789;

        public Candidate Build()
        {
            return new Candidate
            {
                Id = _candidateId,
                LegacyCandidateId = _legacyCandidateId
            };
        }

        public CandidateBuilder WithCandidateId(Guid candidateId)
        {
            _candidateId = candidateId;
            return this;
        }

        public CandidateBuilder WithLegacyCandidateId(int legacyCandidateId)
        {
            _legacyCandidateId = legacyCandidateId;
            return this;
        }
    }
}
namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;
    using Entities.Sql;
    using CandidateSummary = Entities.Sql.CandidateSummary;
    using Candidate = Entities.Sql.Candidate;

    public interface ICandidateMappers
    {
        CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<Guid, CandidateSummary> candidateSummaries);
        Dictionary<string, object> MapCandidateDictionary(Candidate candidate);
        Dictionary<string, object> MapPersonDictionary(Person person);
    }
}
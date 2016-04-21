namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Entities.Mongo;

    public interface ICandidateMappers
    {
        Entities.Sql.Candidate MapCandidate(CandidateUser candidateUser);
        IDictionary<string, object> MapCandidateDictionary(CandidateUser candidateUser);
    }
}
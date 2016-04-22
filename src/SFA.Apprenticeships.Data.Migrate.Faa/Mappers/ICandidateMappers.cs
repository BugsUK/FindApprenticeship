namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;

    public interface ICandidateMappers
    {
        CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<Guid, int> candidateIds, IDictionary<string, int> personIds);
        Dictionary<string, object> MapCandidateDictionary(Entities.Sql.Candidate candidate);
        Dictionary<string, object> MapPersonDictionary(Entities.Sql.Person person);
    }
}
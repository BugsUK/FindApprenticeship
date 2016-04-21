namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Entities;
    using Entities.Mongo;

    public interface ICandidateMappers
    {
        CandidatePerson MapCandidatePerson(CandidateUser candidateUser, IDictionary<string, int> persons);
        CandidatePersonDictionary MapCandidatePersonDictionary(CandidateUser candidateUser, IDictionary<string, int> persons);
    }
}
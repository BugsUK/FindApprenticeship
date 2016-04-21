namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using Entities;
    using Entities.Mongo;

    public interface ICandidateMappers
    {
        CandidatePerson MapCandidatePerson(CandidateUser candidateUser);
        CandidatePersonDictionary MapCandidatePersonDictionary(CandidateUser candidateUser);
    }
}
namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Candidates.Mappers
{
    using Domain.Entities.Candidates;
    using Infrastructure.Common.Mappers;
    using Mongo.Candidates.Entities;

    public class CandidateMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Candidate, MongoCandidate>();
            Mapper.CreateMap<MongoCandidate, Candidate>();

            Mapper.CreateMap<SavedSearch, MongoSavedSearch>();
            Mapper.CreateMap<MongoSavedSearch, SavedSearch>();
        }
    }
}

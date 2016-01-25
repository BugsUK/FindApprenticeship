namespace SFA.Apprenticeships.Infrastructure.Repositories.Candidates.Mappers
{
    using System;
    using SFA.Infrastructure.Interfaces;
    using Common.Mappers;
    using Domain.Entities.Candidates;
    using Entities;

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

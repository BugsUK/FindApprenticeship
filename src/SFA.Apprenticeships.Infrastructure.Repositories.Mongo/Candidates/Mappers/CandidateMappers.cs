namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Candidates.Mappers
{
    using Domain.Entities.Candidates;
    using Infrastructure.Common.Mappers;
    using Entities;

    public class CandidateMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Candidate, MongoCandidate>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<MongoCandidate, Candidate>();

            Mapper.CreateMap<MongoCandidate, CandidateSummary>()
                .ForMember(c => c.FirstName, opt => opt.MapFrom(s => s.RegistrationDetails.FirstName))
                .ForMember(c => c.MiddleNames, opt => opt.MapFrom(s => s.RegistrationDetails.MiddleNames))
                .ForMember(c => c.LastName, opt => opt.MapFrom(s => s.RegistrationDetails.LastName))
                .ForMember(c => c.DateOfBirth, opt => opt.MapFrom(s => s.RegistrationDetails.DateOfBirth))
                .ForMember(c => c.Address, opt => opt.MapFrom(s => s.RegistrationDetails.Address));

            Mapper.CreateMap<SavedSearch, MongoSavedSearch>()
                .ForMember(c => c.Id, opt => opt.Ignore());
            Mapper.CreateMap<MongoSavedSearch, SavedSearch>();
        }
    }
}

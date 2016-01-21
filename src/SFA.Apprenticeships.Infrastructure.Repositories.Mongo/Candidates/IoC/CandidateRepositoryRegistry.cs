namespace SFA.Apprenticeships.Infrastructure.Repositories.Mongo.Candidates.IoC
{
    using Domain.Interfaces.Repositories;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class CandidateRepositoryRegistry : Registry
    {
        public CandidateRepositoryRegistry()
        {
            For<IMapper>().Use<CandidateMappers>().Name = "CandidateMapper";

            For<ICandidateReadRepository>().Use<CandidateRepository>().Ctor<IMapper>().Named("CandidateMapper");
            For<ICandidateWriteRepository>().Use<CandidateRepository>().Ctor<IMapper>().Named("CandidateMapper");

            For<ISavedSearchReadRepository>().Use<SavedSearchRepository>().Ctor<IMapper>().Named("CandidateMapper");
            For<ISavedSearchWriteRepository>().Use<SavedSearchRepository>().Ctor<IMapper>().Named("CandidateMapper");
        }
    }
}

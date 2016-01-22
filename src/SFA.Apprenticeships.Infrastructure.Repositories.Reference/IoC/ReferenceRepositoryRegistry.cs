namespace SFA.Apprenticeships.Infrastructure.Repositories.Reference.IoC
{
    using Domain.Interfaces.Repositories;
    using Reference;
    using Mappers;
    using SFA.Infrastructure.Interfaces;
    using StructureMap.Configuration.DSL;

    public class ReferenceRepositoryRegistry : Registry
    {
        public ReferenceRepositoryRegistry()
        {
            For<IMapper>().Use<ReferenceMappers>().Name = "ProviderMappers";
            //TODO: Switch once we've moved over to new repository
            //For<IReferenceRepository>().Use<SqlReferenceRepository>().Ctor<IMapper>().Named("ProviderMappers");
            For<IReferenceRepository>().Use<TacticalReferenceRepository>();
        }
    }
}
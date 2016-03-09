namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using DatabaseProvider = Entities.Provider;
    using Infrastructure.Common.Mappers;
    using DomainProvider = Domain.Entities.Raa.Parties.Provider;
    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<DatabaseProvider, DomainProvider>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.FullName));
            // .ForMember(dbp => dbp.Ukprn, opt => opt.MapFrom(ep => ep.Ukprn));
        }
    }
}

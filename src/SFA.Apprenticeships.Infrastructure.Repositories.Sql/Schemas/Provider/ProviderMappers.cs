namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using AutoMapper;
    using Infrastructure.Common.Mappers;
    using DatabaseProvider = Entities.Provider;
    using DomainProvider = Domain.Entities.Raa.Parties.Provider;

    public class StringToIntConverter : ValueResolver<string, int>
    {
        protected override int ResolveCore(string source)
        {
            return int.Parse(source);
        }
    }

    public class ProviderToUseFaaToBool : ValueResolver<int?, bool>
    {
        protected override bool ResolveCore(int? source)
        {
            return source == 2;
        }
    }

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<DatabaseProvider, DomainProvider>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.FullName))
                .ForMember(dest => dest.IsMigrated, opt => opt.ResolveUsing<ProviderToUseFaaToBool>().FromMember(source => source.ProviderToUseFAA) );

            Mapper.CreateMap<DomainProvider, DatabaseProvider>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Ukprn,
                    opt => opt.ResolveUsing<StringToIntConverter>().FromMember(source => source.Ukprn))
                .ForMember(dest => dest.ProviderToUseFAA, opt => opt.Ignore());
        }
    }
}

namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using AutoMapper;
    using Domain.Entities.Users;
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

    public class BoolToProviderToUseFaa : ValueResolver<bool, int?>
    {
        protected override int? ResolveCore(bool source)
        {
            return source ? 2 : (int?)null;
        }
    }

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<DatabaseProvider, DomainProvider>()
                .ForMember(dest => dest.IsMigrated, opt => opt.ResolveUsing<ProviderToUseFaaToBool>().FromMember(source => source.ProviderToUseFAA))
                .ForMember(dest => dest.ProviderStatusType, opt => opt.MapFrom(source => source.ProviderStatusTypeId));

            Mapper.CreateMap<DomainProvider, DatabaseProvider>()
                .ForMember(dest => dest.Ukprn,
                    opt => opt.ResolveUsing<StringToIntConverter>().FromMember(source => source.Ukprn))
                .ForMember(dest => dest.ProviderToUseFAA, opt => opt.ResolveUsing<BoolToProviderToUseFaa>().FromMember(src => src.IsMigrated))
                .ForMember(dest => dest.IsContracted, opt => opt.UseValue(true))
                .ForMember(dest => dest.ProviderStatusTypeId, opt => opt.UseValue(ProviderStatuses.Activated))
                .ForMember(dest => dest.IsNasProvider, opt => opt.UseValue(false));
        }
    }
}

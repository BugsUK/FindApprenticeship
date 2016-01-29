namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using AutoMapper;
    using Domain.Entities.Providers;
    using Vacancy;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            //TODO: When implementing changes to domain model, uncomment the id mappings, below
            Mapper.CreateMap<Guid, int>().ConvertUsing<GuidToIntConverter>();
            Mapper.CreateMap<int, Guid>().ConvertUsing<IntToGuidConverter>();

            Mapper.CreateMap<Provider, Entities.Provider>()
                .ForMember(destination => destination.ProviderId, opt => opt.MapFrom(source => source.EntityId))
                .ForMember(destintation => destintation.FullName, opt => opt.MapFrom(source => source.Name));
            
            Mapper.CreateMap<Entities.Provider, Provider>()
                .ForMember(destination => destination.EntityId, opt => opt.MapFrom(source => source.ProviderId))
                .ForMember(destintation => destintation.Name, opt => opt.MapFrom(source => source.FullName));
        }
    }

    public class GuidToIntConverter :
        ITypeConverter<Guid, int>
    {
        public int Convert(ResolutionContext context)
        {
            var source = (Guid)context.SourceValue;
            return source.ResolveToInt();
        }
    }

    public class IntToGuidConverter :
        ITypeConverter<int, Guid>
    {
        public Guid Convert(ResolutionContext context)
        {
            var source = (int)context.SourceValue;
            return source.ResolveToGuid();
        }
    }

    public class GuidToIntResolver : ValueResolver<Guid, int>
    {
        protected override int ResolveCore(Guid source)
        {
            return source.ResolveToInt();
        }
    }

    public static class GuidToIntResolverHelper
    {
        public static int ResolveToInt(this Guid guid)
        {
            return int.Parse(guid.ToString().Replace("-", ""));
        }
    }

    public class IntToGuidResolver : ValueResolver<int, Guid>
    {
        protected override Guid ResolveCore(int source)
        {
            return source.ResolveToGuid();
        }
    }

    public static class IntToGuidResolverHelper
    {
        public static Guid ResolveToGuid(this int source)
        {
            var stringValue = source.ToString("00000000-0000-0000-0000-000000000000");
            return new Guid(stringValue);
        }
    }
}

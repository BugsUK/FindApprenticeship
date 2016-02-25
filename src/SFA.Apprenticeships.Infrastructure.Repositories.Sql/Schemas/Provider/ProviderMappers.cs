namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Provider
{
    using System;
    using Domain.Entities.Raa.Parties;
    using Vacancy;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Provider, Entities.Provider>()
                .ForMember(destintation => destintation.FullName, opt =>
                    opt.MapFrom(source => source.Name))

                .ForMember(destination => destination.Ukprn, opt =>
                    opt.MapFrom(source => Convert.ToInt32(source.Ukprn)));

            Mapper.CreateMap<Entities.Provider, Provider>()
                .ForMember(destintation => destintation.Name, opt =>
                    opt.MapFrom(source => source.FullName))

                .ForMember(destination => destination.Ukprn, opt =>
                    opt.MapFrom(source => Convert.ToString(source.Ukprn)));
        }
    }

    // TODO: SQL: AG: remove dead code below.

    /*
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
    public static class GuidToIntResolverHelper
    {
        public static int ResolveToInt(this Guid guid)
        {
            //return int.Parse(guid.ToString().Replace("-", ""));
            byte[] b = guid.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }
    }
    public static class IntToGuidResolverHelper
    {
        public static Guid ResolveToGuid(this int source)
        {
            //var stringValue = source.ToString("00000000-0000-0000-0000-000000000000");
            //return new Guid(stringValue);
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(source).CopyTo(bytes, 0);
            return new Guid(bytes);
        }
    }
    */
}
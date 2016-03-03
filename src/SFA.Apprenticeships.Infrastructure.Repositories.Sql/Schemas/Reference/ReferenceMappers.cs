namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using Domain.Entities.Raa.Reference;
    using Infrastructure.Common.Mappers;

    public class ReferenceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<County, Entities.County>()
                .ForMember(c => c.PostalAddresses, opt => opt.Ignore());
            Mapper.CreateMap<Entities.County, County>();

            Mapper.CreateMap<Region, Entities.Region>();
            Mapper.CreateMap<Entities.Region, Region>()
                .ForMember(c => c.CodeName, opt => opt.MapFrom(r => r.CodeName.Trim()))
                .ForMember(c => c.ShortName, opt => opt.MapFrom(r => r.ShortName.Trim()));

            Mapper.CreateMap<LocalAuthority, Entities.LocalAuthority>();
            Mapper.CreateMap<Entities.LocalAuthority, LocalAuthority>();
        }
    }
}
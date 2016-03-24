namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Entities;
    using Infrastructure.Common.Mappers;
    //using County = Domain.Entities.Raa.Reference.County;
    using Framework = Domain.Entities.Raa.Reference.Framework;
    //using LocalAuthority = Domain.Entities.Raa.Reference.LocalAuthority;
    using Occupation = Domain.Entities.Raa.Reference.Occupation;
    using Sector = Domain.Entities.Raa.Vacancies.Sector;
    using Standard = Domain.Entities.Raa.Vacancies.Standard;

    public class ReferenceMappers : MapperEngine
    {
        public override void Initialise()
        {
            //Mapper.CreateMap<County, Entities.County>()
            //    .ForMember(c => c.PostalAddresses, opt => opt.Ignore());
            //Mapper.CreateMap<Entities.County, County>();

            //Mapper.CreateMap<Region, Entities.Region>();
            //Mapper.CreateMap<Entities.Region, Region>()
            //    .ForMember(c => c.CodeName, opt => opt.MapFrom(r => r.CodeName.Trim()))
            //    .ForMember(c => c.ShortName, opt => opt.MapFrom(r => r.ShortName.Trim()));

            //Mapper.CreateMap<LocalAuthority, Entities.LocalAuthority>();
            //Mapper.CreateMap<Entities.LocalAuthority, LocalAuthority>();

            Mapper.CreateMap<Sector, Entities.StandardSector>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StandardSectorId, opt => opt.MapFrom(src => src.Id));
            Mapper.CreateMap<Entities.StandardSector, Sector>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StandardSectorId))
                .ForMember(dest => dest.Standards, opt => opt.Ignore());


            Mapper.CreateMap<Framework, Entities.ApprenticeshipFramework>()
                .ForMember(dest => dest.ApprenticeshipFrameworkId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClosedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ApprenticeshipFrameworkStatusTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.PreviousApprenticeshipOccupationId, opt => opt.Ignore())
                .ForMember(dest => dest.ApprenticeshipOccupationId, opt => opt.Ignore());
            Mapper.CreateMap<Entities.ApprenticeshipFramework, Framework>()
                .ForMember(dest => dest.Occupation, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApprenticeshipFrameworkId));

            Mapper.CreateMap<Occupation, Entities.ApprenticeshipOccupation>()
                .ForMember(dest => dest.ApprenticeshipOccupationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApprenticeshipOccupationStatusTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.ClosedDate, opt => opt.Ignore());

            Mapper.CreateMap<Entities.ApprenticeshipOccupation, Occupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApprenticeshipOccupationId));
        }
    }
}
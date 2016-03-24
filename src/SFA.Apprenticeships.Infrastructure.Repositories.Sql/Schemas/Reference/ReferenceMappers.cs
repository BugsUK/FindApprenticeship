namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using System;
    using Domain.Entities.Raa.Vacancies;
    using Entities;
    using Infrastructure.Common.Mappers;
    using County = Domain.Entities.Raa.Reference.County;
    using Framework = Domain.Entities.Raa.Reference.Framework;
    using LocalAuthority = Domain.Entities.Raa.Reference.LocalAuthority;
    using Occupation = Domain.Entities.Raa.Reference.Occupation;
    using Sector = Domain.Entities.Raa.Vacancies.Sector;
    using Standard = Domain.Entities.Raa.Vacancies.Standard;

    public class ReferenceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<County, Entities.County>()
                .ForMember(c => c.PostalAddresses, opt => opt.Ignore());
            Mapper.CreateMap<Entities.County, County>();

            //Mapper.CreateMap<Region, Entities.Region>();
            //Mapper.CreateMap<Entities.Region, Region>()
            //    .ForMember(c => c.CodeName, opt => opt.MapFrom(r => r.CodeName.Trim()))
            //    .ForMember(c => c.ShortName, opt => opt.MapFrom(r => r.ShortName.Trim()));

            Mapper.CreateMap<LocalAuthority, Entities.LocalAuthority>();
            Mapper.CreateMap<Entities.LocalAuthority, LocalAuthority>();

            Mapper.CreateMap<Sector, Entities.Sector>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SectorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Standards, opt => opt.Ignore());
            Mapper.CreateMap<Entities.Sector, Sector>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SectorId))
                .ForMember(dest => dest.Standards, opt => opt.Ignore());


            Mapper.CreateMap<Framework, Entities.Framework>()
                .ForMember(dest => dest.FrameworkId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClosedDate, opt => opt.Ignore())
                .ForMember(dest => dest.FrameworkStatus, opt => opt.Ignore())
                .ForMember(dest => dest.FrameworkStatusId, opt => opt.Ignore())
                .ForMember(dest => dest.Occupation1, opt => opt.Ignore())
                .ForMember(dest => dest.PreviousOccupationId, opt => opt.Ignore())
                .ForMember(dest => dest.Occupation, opt => opt.Ignore())
                .ForMember(dest => dest.OccupationId, opt => opt.Ignore())
                .ForMember(dest => dest.Vacancies, opt => opt.Ignore());
            Mapper.CreateMap<Entities.Framework, Framework>()
                .ForMember(dest => dest.Occupation, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FrameworkId));

            Mapper.CreateMap<Standard, Entities.Standard>()
                .ForMember(dest => dest.StandardId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Level,
                    opt => opt.MapFrom(src => new Level() {LevelCode = src.ApprenticeshipLevel.ToString("D")}))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.SectorId, opt => opt.MapFrom(src => src.ApprenticeshipSectorId))
                .ForMember(dest => dest.Sector, opt => opt.Ignore())
                .ForMember(dest => dest.Vacancies, opt => opt.Ignore())
                .ForMember(dest => dest.LevelCode, opt => opt.MapFrom(src=> src.ApprenticeshipLevel.ToString("D")));
            Mapper.CreateMap<Entities.Standard, Standard>()
                .ForMember(dest => dest.ApprenticeshipLevel, opt => opt.MapFrom(src => Enum.Parse(typeof(ApprenticeshipLevel), src.Level.LevelCode)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ApprenticeshipSectorId, opt => opt.MapFrom(src => src.SectorId))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StandardId));

            Mapper.CreateMap<Occupation, Entities.Occupation>()
                .ForMember(dest => dest.OccupationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClosedDate, opt => opt.Ignore())
                .ForMember(dest => dest.Frameworks, opt => opt.Ignore())
                .ForMember(dest => dest.Frameworks1, opt => opt.Ignore())
                .ForMember(dest => dest.OccupationStatus, opt => opt.Ignore())
                .ForMember(dest => dest.OccupationStatusId, opt => opt.Ignore());

            Mapper.CreateMap<Entities.Occupation, Occupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OccupationId));
        }
    }
}
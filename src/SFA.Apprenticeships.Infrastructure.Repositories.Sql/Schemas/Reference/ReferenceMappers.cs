namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Reference
{
    using Entities;
    using Infrastructure.Common.Mappers;
    using DomainFramework = Domain.Entities.Raa.Reference.Framework;
    using DomainOccupation = Domain.Entities.Raa.Reference.Occupation;
    using DomainSector = Domain.Entities.Raa.Vacancies.Sector;
    using Standard = Domain.Entities.Raa.Vacancies.Standard;

    public class ReferenceMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<DomainSector, Entities.StandardSector>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.StandardSectorId, opt => opt.MapFrom(src => src.Id));
            Mapper.CreateMap<Entities.StandardSector, DomainSector>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.StandardSectorId))
                .ForMember(dest => dest.Standards, opt => opt.Ignore());
            Mapper.CreateMap<DomainFramework, Entities.ApprenticeshipFramework>()
                .ForMember(dest => dest.ApprenticeshipFrameworkId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ClosedDate, opt => opt.Ignore())
                .ForMember(dest => dest.ApprenticeshipFrameworkStatusTypeId, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.PreviousApprenticeshipOccupationId, opt => opt.Ignore())
                .ForMember(dest => dest.ApprenticeshipOccupationId, opt => opt.Ignore());
            Mapper.CreateMap<Entities.ApprenticeshipFramework, DomainFramework>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApprenticeshipFrameworkId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ApprenticeshipFrameworkStatusTypeId))
                .ForMember(dest => dest.ParentCategoryCodeName, opt => opt.Ignore());
            Mapper.CreateMap<DomainOccupation, Entities.ApprenticeshipOccupation>()
                .ForMember(dest => dest.ApprenticeshipOccupationId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ApprenticeshipOccupationStatusTypeId, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.ClosedDate, opt => opt.Ignore());
            Mapper.CreateMap<Entities.ApprenticeshipOccupation, DomainOccupation>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ApprenticeshipOccupationId))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ApprenticeshipOccupationStatusTypeId))
                .ForMember(dest => dest.Frameworks, opt => opt.Ignore());
            Mapper.CreateMap<Standard, Entities.Standard>()
                .ForMember(dest => dest.EducationLevelId, opt => opt.MapFrom(src => (int) src.ApprenticeshipLevel))
                .ForMember(dest => dest.StandardSectorId, opt => opt.MapFrom(src => src.ApprenticeshipSectorId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<Entities.Standard, Standard>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.ApprenticeshipSectorId, opt => opt.MapFrom(src => src.StandardSectorId))
                .ForMember(dest => (int)dest.ApprenticeshipLevel, opt => opt.MapFrom(src => src.EducationLevelId));
        }
    }
}
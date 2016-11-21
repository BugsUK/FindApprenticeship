namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Infrastructure.Common.Mappers;
    using VacancyOwnerRelationship = Domain.Entities.Raa.Parties.VacancyOwnerRelationship;

    public class VacancyOwnerRelationshipMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Entities.VacancyOwnerRelationship, VacancyOwnerRelationship>()
                .ForMember(v => v.VacancyOwnerRelationshipGuid, opt => opt.Ignore())
                .ForMember(v => v.VacancyOwnerRelationshipId, opt => opt.MapFrom(src => src.VacancyOwnerRelationshipId))
                .ForMember(v => v.ProviderSiteId, opt => opt.MapFrom(src => src.ProviderSiteID))
                .ForMember(v => v.EmployerWebsiteUrl, opt => opt.MapFrom(src => src.EmployerWebsite));
              

            Mapper.CreateMap<VacancyOwnerRelationship, Entities.VacancyOwnerRelationship>()
                .ForMember(v => v.VacancyOwnerRelationshipId, opt => opt.MapFrom(src => src.VacancyOwnerRelationshipId))
                .ForMember(v => v.ProviderSiteID, opt => opt.MapFrom(src => src.ProviderSiteId))
                .ForMember(v => v.EmployerWebsite, opt => opt.MapFrom(src => src.EmployerWebsiteUrl))
                .ForMember(v => v.ContractHolderIsEmployer, opt => opt.Ignore())
                .ForMember(v => v.ManagerIsEmployer, opt => opt.Ignore())
                .ForMember(v => v.StatusTypeId, opt => opt.Ignore())
                .ForMember(v => v.Notes, opt => opt.Ignore())
                .ForMember(v => v.EmployerLogoAttachmentId, opt => opt.Ignore())
                .ForMember(v => v.NationWideAllowed, opt => opt.Ignore())
                .ForMember(v => v.EditedInRaa, opt => opt.Ignore());
        }
    }
}
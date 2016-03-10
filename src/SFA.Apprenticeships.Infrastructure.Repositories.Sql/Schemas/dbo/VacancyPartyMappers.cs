namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.dbo
{
    using Domain.Entities.Raa.Parties;
    using Entities;
    using Infrastructure.Common.Mappers;

    public class VacancyPartyMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<VacancyOwnerRelationship, VacancyParty>()
                .ForMember(v => v.VacancyPartyId, opt => opt.MapFrom(src => src.VacancyOwnerRelationshipId))
                .ForMember(v => v.ProviderSiteId, opt => opt.MapFrom(src => src.ProviderSiteID))
                .ForMember(v => v.EmployerWebsiteUrl, opt => opt.MapFrom(src => src.EmployerWebsite));

            Mapper.CreateMap<VacancyParty, VacancyOwnerRelationship>()
                .ForMember(v => v.VacancyOwnerRelationshipId, opt => opt.MapFrom(src => src.VacancyPartyId))
                .ForMember(v => v.ProviderSiteID, opt => opt.MapFrom(src => src.ProviderSiteId))
                .ForMember(v => v.EmployerWebsite, opt => opt.MapFrom(src => src.EmployerWebsiteUrl));
        }
    }
}
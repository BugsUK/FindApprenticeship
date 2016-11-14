namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Parties;
    using ViewModels.Provider;

    public class ProviderMappers : RaaCommonWebMappers
    {
        public override void Initialise()
        {
            base.Initialise();

            Mapper.CreateMap<Provider, ProviderViewModel>()
                .ForMember(dest => dest.ProviderSiteViewModels, opt => opt.Ignore())
                .ForMember(dest => dest.VacanciesReferenceNumbers, opt => opt.Ignore());
            Mapper.CreateMap<ProviderViewModel, Provider>();

            Mapper.CreateMap<ProviderSite, ProviderSiteViewModel>()
                .ForMember(dest => dest.ProviderId, opt => opt.Ignore())
                .ForMember(dest => dest.ProviderSiteRelationships, opt => opt.Ignore())
                .ForMember(dest => dest.ProviderUkprn, opt => opt.Ignore())
                .ForMember(dest => dest.ProviderSiteRelationshipType, opt => opt.Ignore());
            Mapper.CreateMap<ProviderSiteViewModel, ProviderSite>()
                .ForMember(dest => dest.ProviderSiteRelationships, opt => opt.Ignore());
        }
    }
}
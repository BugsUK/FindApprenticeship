namespace SFA.Apprenticeships.Web.Raa.Common.Mappers
{
    using Domain.Entities.Raa.Parties;
    using Infrastructure.Common.Mappers;
    using ViewModels.Provider;

    public class ProviderMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<Provider, ProviderViewModel>()
                .ForMember(dest => dest.ProviderSiteViewModels, opt => opt.Ignore());
            Mapper.CreateMap<ProviderViewModel, Provider>();
        }
    }
}
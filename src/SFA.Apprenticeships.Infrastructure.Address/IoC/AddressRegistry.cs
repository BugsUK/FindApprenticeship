namespace SFA.Apprenticeships.Infrastructure.Address.IoC
{
    using Application.Address;
    using Configuration;
    using Domain.Interfaces.Configuration;
    using Domain.Interfaces.Mapping;
    using Mappers;
    using RestSharp;
    using StructureMap.Configuration.DSL;

    public class AddressRegistry : Registry
    {
        public AddressRegistry(IConfigurationService configurationService)
        {
            var configuration = configurationService.Get<AddressSearchConfiguration>();

            switch (configuration.Provider)
            {
                case "ElasticIndex":
                {
                    For<IMapper>().Use<AddressMapper>().Name = "AddressMapper";
                    For<IAddressSearchProvider>().Use<AddressSearchProvider>().Ctor<IMapper>().Named("AddressMapper");
                    break;
                }
                case "OrdnanceSurveyPlaces":
                {
                    For<IAddressSearchProvider>().Use<OrdnanceSurveyAddressSearchProvider>().Ctor<IRestClient>().Is(new RestClient());
                    break;
                }
            }
        }
    }
}

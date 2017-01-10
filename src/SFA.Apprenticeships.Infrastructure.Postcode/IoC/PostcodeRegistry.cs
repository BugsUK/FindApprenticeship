namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using Application.Interfaces.Locations;
    using Application.Location;
    using Strategies;
    using StructureMap.Configuration.DSL;

    public class PostcodeRegistry : Registry
    {
        public PostcodeRegistry()
        {
            For<IPostcodeLookupProvider>().Use<PostcodeLookupProvider>();
            For<IAddressLookupProvider>().Use<AddressLookupProvider>();
            For<IFindPostcodeService>().Use<FindPostcodeService>();
            For<IRetrieveAddressService>().Use<RetrieveAddressService>();
            For<IPostalAddressDetailsService>().Use<PostalAddressDetailsService>();
            For<IPostalAddressLookupProvider>().Use<PostalAddressLookupProvider>();
            For<IPostalAddressSearchService>().Use<PostalAddressSearchService>();
            For<IGeoCodeLookupProvider>().Use<GeoCodeLookupProvider>();
            For<ILocalAuthorityLookupProvider>().Use<LocalAuthorityLookupProvider>();
            For<IPostalAddressStrategy>().Use<PostalAddressStrategy>();
        }
    }
}

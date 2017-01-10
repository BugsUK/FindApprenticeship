namespace SFA.Apprenticeships.Infrastructure.Postcode.IoC
{
    using Application.Interfaces.Locations;
    using Application.Location;
    using Application.Location.Strategies;
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
#pragma warning disable 618
#pragma warning disable 612
            For<IPostalAddressSearchService>().Use<PostalAddressSearchService>();
#pragma warning restore 612
#pragma warning restore 618
            For<IGeoCodeLookupProvider>().Use<GeoCodeLookupProvider>();
            For<ILocalAuthorityLookupProvider>().Use<LocalAuthorityLookupProvider>();
            For<IPostalAddressStrategy>().Use<PostalAddressStrategy>();
        }
    }
}

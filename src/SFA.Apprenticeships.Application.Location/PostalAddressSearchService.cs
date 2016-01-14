namespace SFA.Apprenticeships.Application.Location
{
    using System;
    using System.Linq;
    using Domain.Entities.Locations;
    using Interfaces.Locations;
    public class PostalAddressSearchService : IPostalAddressSearchService
    {
        private IPostalAddressLookupProvider _postalAddressLookupProvider;

        public PostalAddressSearchService(IPostalAddressLookupProvider postalAddressLookupProvider)
        {
            _postalAddressLookupProvider = postalAddressLookupProvider;
        }

        public PostalAddress GetAddress(string fullPostcode, string addressLine1)
        {
            var results = _postalAddressLookupProvider.GetPostalAddresses(addressLine1, fullPostcode);

            return results?.Single();
        }
    }
}

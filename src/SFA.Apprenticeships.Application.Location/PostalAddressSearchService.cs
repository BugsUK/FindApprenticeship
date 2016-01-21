namespace SFA.Apprenticeships.Application.Location
{
    using System.Collections.Generic;
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

        public PostalAddress GetValidatedAddress(string fullPostcode, string addressLine1)
        {
            var results = _postalAddressLookupProvider.GetValidatedPostalAddresses(addressLine1, fullPostcode);

            return results?.Single();
        }

        public IEnumerable<PostalAddress> GetValidatedAddresses(string fullPostcode)
        {
            var results = _postalAddressLookupProvider.GetValidatedPostalAddresses(fullPostcode);

            return results;
        }
    }
}

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

        public IEnumerable<PostalAddress> GetValidatedAddress(string fullPostcode, string addressLine1)
        {
            var results = _postalAddressLookupProvider.GetValidatedPostalAddresses(addressLine1, fullPostcode);

            if (results == null)
                return null;
            return !results.Any() ? null : results;
        }

        public IEnumerable<PostalAddress> GetValidatedAddresses(string fullPostcode)
        {
            var results = _postalAddressLookupProvider.GetValidatedPostalAddresses(fullPostcode);

            if (results == null)
                return null;

            return !results.Any() ? null : results;
        }
    }
}

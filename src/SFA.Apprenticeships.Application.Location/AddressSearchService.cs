namespace SFA.Apprenticeships.Application.Location
{
    using System.Collections.Generic;
    using System.Linq;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Interfaces.Generic;
    using Interfaces.Locations;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IAddressLookupProvider _addressLookupProvider;

        public AddressSearchService(IAddressLookupProvider addressLookupProvider )
        {
            _addressLookupProvider = addressLookupProvider;
        }

        public Pageable<PostalAddress> GetAddressesFor(string fullPostcode, int currentPage, int pageSize)
        {
            Condition.Requires(fullPostcode, "placeNameOrPostcode").IsNotNullOrWhiteSpace();

            if (!LocationHelper.IsPostcode(fullPostcode))
            {
                var message = $"{fullPostcode} is not a full postcode.";
                throw new CustomException(message, ErrorCodes.AddressLookupFailed);
            }

            var addresses = _addressLookupProvider.GetPossibleAddresses(fullPostcode);

            var addressesPage = new Pageable<PostalAddress>
            {
                Page = addresses.Skip((currentPage - 1) * pageSize).Take(pageSize),
                ResultsCount = addresses.Count(),
                CurrentPage = currentPage,
                TotalNumberOfPages = ( addresses.Count() / pageSize ) + 1
            };
            return addressesPage;
        }
    }
}
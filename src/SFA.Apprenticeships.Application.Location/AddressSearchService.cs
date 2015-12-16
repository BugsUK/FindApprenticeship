namespace SFA.Apprenticeships.Application.Location
{
    using System.Collections.Generic;
    using CuttingEdge.Conditions;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Interfaces.Locations;

    public class AddressSearchService : IAddressSearchService
    {
        private readonly IAddressLookupProvider _addressLookupProvider;

        public AddressSearchService(IAddressLookupProvider addressLookupProvider )
        {
            _addressLookupProvider = addressLookupProvider;
        }

        public IEnumerable<Address> GetAddressesFor(string fullPostcode)
        {
            Condition.Requires(fullPostcode, "placeNameOrPostcode").IsNotNullOrWhiteSpace();

            if (!LocationHelper.IsPostcode(fullPostcode))
            {
                var message = $"{fullPostcode} is not a full postcode.";
                throw new CustomException(message, ErrorCodes.AddressLookupFailed);
            }

            return _addressLookupProvider.GetPossibleAddresses(fullPostcode);
        }
    }
}
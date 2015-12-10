namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Logging;
    using Application.Location;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;

    public class AddressLookupProvider : IAddressLookupProvider
    {
        private readonly ILogService _logger;

        private readonly FindPostcodeService _findPostcodeService;
        private readonly RetrieveAddressService _retrieveAddressService;


        public AddressLookupProvider(ILogService logger, FindPostcodeService findPostcodeService, RetrieveAddressService retrieveAddressService)
        {
            _logger = logger;
            _findPostcodeService = findPostcodeService;
            _retrieveAddressService = retrieveAddressService;
        }

        public IEnumerable<Address> GetPossibleAddresses(string postcode)
        {
            Condition.Requires(postcode, "postcode").IsNotNullOrWhiteSpace();

            _logger.Debug("Calling GetPossibleAddresses for Postcode={0}", postcode);

            var possiblePostcodes = _findPostcodeService.FindPostcodes(postcode);

            return possiblePostcodes.Select(postcodeSearchInfo => _retrieveAddressService.RetrieveAddress(postcodeSearchInfo.Id));
        }
    }
}
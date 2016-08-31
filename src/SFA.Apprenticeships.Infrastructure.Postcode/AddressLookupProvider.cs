namespace SFA.Apprenticeships.Infrastructure.Postcode
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Location;
    using SFA.Infrastructure.Interfaces;
    using CuttingEdge.Conditions;
    using Domain.Entities.Locations;

    using SFA.Apprenticeships.Application.Interfaces;

    public class AddressLookupProvider : IAddressLookupProvider
    {
        private readonly ILogService _logger;

        private readonly IFindPostcodeService _findPostcodeService;
        private readonly IRetrieveAddressService _retrieveAddressService;


        public AddressLookupProvider(ILogService logger, IFindPostcodeService findPostcodeService, IRetrieveAddressService retrieveAddressService)
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

            return possiblePostcodes
                .Where(postcodeSearchInfo => postcodeSearchInfo.Id != null)
                .Select(postcodeSearchInfo => _retrieveAddressService.RetrieveAddress(postcodeSearchInfo.Id));
        }
    }
}
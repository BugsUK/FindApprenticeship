namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using ViewModels.VacancyPosting;
    using Web.Common.ViewModels.Locations;

    public class LocationsProvider : ILocationsProvider
    {
        private readonly IAddressSearchService _addressSearchService;

        public LocationsProvider(IAddressSearchService addressSearchService)
        {
            _addressSearchService = addressSearchService;
        }

        public List<VacancyLocationAddressViewModel> GetAddressesFor(string fullPostcode)
        {
            var possibleAddresses = _addressSearchService.GetAddressesFor(fullPostcode);

            var result = new List<VacancyLocationAddressViewModel>();

            possibleAddresses.ToList()
                .ForEach(a => result.Add(new VacancyLocationAddressViewModel
                {
                    Address = new AddressViewModel
                    {
                        AddressLine1 = a.AddressLine1,
                        AddressLine2 = a.AddressLine2,
                        AddressLine3 = a.AddressLine3,
                        AddressLine4 = a.AddressLine4,
                        Postcode = a.Postcode,
                        Uprn = a.Uprn
                    }
                }));

            return result;
        }
    }
}
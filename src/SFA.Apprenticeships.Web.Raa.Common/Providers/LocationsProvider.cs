namespace SFA.Apprenticeships.Web.Raa.Common.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using Application.Interfaces.Locations;
    using Domain.Entities.Locations;
    using ViewModels.VacancyPosting;
    using Web.Common.Converters;
    using Web.Common.ViewModels.Locations;

    public class LocationsProvider : ILocationsProvider
    {
        private readonly IAddressSearchService _addressSearchService;

        public LocationsProvider(IAddressSearchService addressSearchService)
        {
            _addressSearchService = addressSearchService;
        }

        public LocationSearchViewModel GetAddressesFor(LocationSearchViewModel viewModel)
        {
            var pageSize = int.MaxValue; // TODO: maybe we can remove the pageable propery. We'll keep it until the design is confirmed.
            var resultsPage = _addressSearchService.GetAddressesFor(viewModel.PostcodeSearch, viewModel.CurrentPage, pageSize);
            var resultsViewModelPage = resultsPage.ToViewModel(resultsPage.Page.Select(ConvertToViewModel));
            viewModel.SearchResultAddresses = resultsViewModelPage;

            return viewModel;
        }

        private string GetAddressLineValue(string addressLine)
        {
            return string.IsNullOrWhiteSpace(addressLine) ? null : addressLine;
        }

        private VacancyLocationAddressViewModel ConvertToViewModel(Address address)
        {
            return new VacancyLocationAddressViewModel
            {
                Address = new AddressViewModel
                {
                    AddressLine1 = GetAddressLineValue(address.AddressLine1),
                    AddressLine2 = GetAddressLineValue(address.AddressLine2),
                    AddressLine3 = GetAddressLineValue(address.AddressLine3),
                    AddressLine4 = GetAddressLineValue(address.AddressLine4),
                    Postcode = address.Postcode,
                    Uprn = address.Uprn
                }
            };
        }
    }
}
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
            var pageSize = 20;
            var resultsPage = _addressSearchService.GetAddressesFor(viewModel.PostcodeSearch, viewModel.CurrentPage, pageSize);
            var resultsViewModelPage = resultsPage.ToViewModel(resultsPage.Page.Select(ConvertToViewModel));
            viewModel.SearchResultAddresses = resultsViewModelPage;

            return viewModel;
        }

        private VacancyLocationAddressViewModel ConvertToViewModel(Address address)
        {
            return new VacancyLocationAddressViewModel
            {
                Address = new AddressViewModel
                {
                    AddressLine1 = address.AddressLine1,
                    AddressLine2 = address.AddressLine2,
                    AddressLine3 = address.AddressLine3,
                    AddressLine4 = address.AddressLine4,
                    Postcode = address.Postcode,
                    Uprn = address.Uprn
                }
            };
        }
    }
}
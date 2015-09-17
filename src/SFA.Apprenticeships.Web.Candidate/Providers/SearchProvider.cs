namespace SFA.Apprenticeships.Web.Candidate.Providers
{
    using System;
    using System.Collections.Generic;
    using Application.Interfaces.Locations;
    using Application.Interfaces.Logging;
    using Constants.Pages;
    using Domain.Entities.Exceptions;
    using Domain.Entities.Locations;
    using Domain.Interfaces.Mapping;
    using ViewModels.VacancySearch;

    public class SearchProvider : ISearchProvider
    {
        private readonly ILocationSearchService _locationSearchService;
        private readonly IMapper _apprenticeshipSearchMapper;
        private readonly ILogService _logger;

        public SearchProvider(
            ILocationSearchService locationSearchService,
            IMapper apprenticeshipSearchMapper,
            ILogService logger)
        {
            _locationSearchService = locationSearchService;
            _apprenticeshipSearchMapper = apprenticeshipSearchMapper;
            _logger = logger;
        }

        public LocationsViewModel FindLocation(string placeNameOrPostcode)
        {
            _logger.Debug("Calling SearchProvider to find the location for place name or postcode: {0}", placeNameOrPostcode);

            try
            {
                var locations = _locationSearchService.FindLocation(placeNameOrPostcode);

                if (locations == null)
                {
                    return new LocationsViewModel();
                }

                return new LocationsViewModel(
                    _apprenticeshipSearchMapper.Map<IEnumerable<Location>, IEnumerable<LocationViewModel>>(locations));
            }
            catch (CustomException e)
            {
                string message, errorMessage;

                switch (e.Code)
                {
                    case ErrorCodes.LocationLookupFailed:
                        errorMessage = string.Format("Location lookup failed for place name {0}", placeNameOrPostcode);
                        message = VacancySearchResultsPageMessages.LocationLookupFailed;
                        break;

                    default:
                        errorMessage = string.Format("Postcode lookup failed for postcode {0}", placeNameOrPostcode);
                        message = VacancySearchResultsPageMessages.PostcodeLookupFailed;
                        break;
                }

                _logger.Error(errorMessage, e);
                return new LocationsViewModel(message);
            }
            catch (Exception e)
            {
                var message = string.Format("Find location failed for place name or postcode {0}", placeNameOrPostcode);
                _logger.Error(message, e);
                throw;
            }
        }
    }
}
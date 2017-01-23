using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.Vacancies;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Constants;
    using Domain.Entities.Vacancies;
    using Extensions;
    using Providers;
    using System;
    using System.Globalization;
    using System.Linq;
    using Validators;
    using ViewModels.VacancySearch;

    public class TraineeshipSearchMediator : SearchMediatorBase, ITraineeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly TraineeshipSearchViewModelServerValidator _searchRequestValidator;
        private readonly TraineeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly ITraineeshipVacancyProvider _traineeshipVacancyProvider;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IGoogleMapsProvider _googleMapsProvider;

        public TraineeshipSearchMediator(
            IConfigurationService configService,
            ISearchProvider searchProvider,
            IUserDataProvider userDataProvider,
            TraineeshipSearchViewModelServerValidator searchRequestValidator,
            TraineeshipSearchViewModelLocationValidator searchLocationValidator,
            ITraineeshipVacancyProvider traineeshipVacancyProvider,
            ICandidateServiceProvider candidateServiceProvider, IGoogleMapsProvider googleMapsProvider)
            : base(configService, userDataProvider)
        {
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _traineeshipVacancyProvider = traineeshipVacancyProvider;
            _candidateServiceProvider = candidateServiceProvider;
            _googleMapsProvider = googleMapsProvider;
        }

        public MediatorResponse<TraineeshipSearchViewModel> Index(Guid? candidateId)
        {
            var lastSearchedLocation = UserDataProvider.Get(UserDataItemNames.LastSearchedLocation);
            if (string.IsNullOrWhiteSpace(lastSearchedLocation) && candidateId.HasValue)
            {
                var candidate = _candidateServiceProvider.GetCandidate(candidateId.Value);
                UserDataProvider.Push(UserDataItemNames.LastSearchedLocation, lastSearchedLocation = candidate.RegistrationDetails.Address.Postcode);
            }

            var traineeshipSearchViewModel = new TraineeshipSearchViewModel
            {
                WithinDistance = 40,
                Distances = GetDistances(false),
                SortTypes = GetSortTypes(),
                SortType = VacancySearchSortType.Distance,
                ResultsPerPage = GetResultsPerPage(),
                Location = SplitSearchLocation(lastSearchedLocation, 0),
                Latitude = SplitSearchLocation(lastSearchedLocation, 1).GetValueOrNull<double>(),
                Longitude = SplitSearchLocation(lastSearchedLocation, 2).GetValueOrNull<double>(),
            };

            return GetMediatorResponse(TraineeshipSearchMediatorCodes.Index.Ok, traineeshipSearchViewModel);
        }

        public MediatorResponse<TraineeshipSearchViewModel> SearchValidation(TraineeshipSearchViewModel model)
        {
            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.SearchValidation.ValidationError, model, clientResult);
            }
            return GetMediatorResponse(TraineeshipSearchMediatorCodes.SearchValidation.Ok, model);
        }

        public MediatorResponse<TraineeshipSearchResponseViewModel> Results(TraineeshipSearchViewModel model)
        {
            UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            UserDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));

            model.Distances = GetDistances(false);
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.ValidationError,
                    new TraineeshipSearchResponseViewModel { VacancySearch = model },
                    clientResult);
            }

            if (!HasGeoPoint(model))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                if (model.Location != null)
                {
                    var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                    if (suggestedLocations.HasError())
                    {
                        return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.HasError, new TraineeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
                    }

                    if (suggestedLocations.Locations.Any())
                    {
                        var location = suggestedLocations.Locations.First();

                        model.Location = location.Name;
                        model.Latitude = location.Latitude;
                        model.Longitude = location.Longitude;

                        model.LocationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
                        {
                            var vsvm = new TraineeshipSearchViewModel
                            {
                                Location = each.Name,
                                Latitude = each.Latitude,
                                Longitude = each.Longitude,
                                PageNumber = model.PageNumber,
                                SortType = model.SortType,
                                WithinDistance = model.WithinDistance,
                                ResultsPerPage = model.ResultsPerPage
                            };

                            vsvm.Hash = vsvm.LatLonLocHash();

                            return vsvm;
                        }).ToArray();
                    }
                }
            }

            var locationResult = _searchLocationValidator.Validate(model);

            if (!locationResult.IsValid)
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.Ok, new TraineeshipSearchResponseViewModel { VacancySearch = model });
            }

            UserDataProvider.Push(UserDataItemNames.LastSearchedLocation, string.Join("|", model.Location, model.Latitude, model.Longitude));

            var traineeshipSearchResponseViewModel = _traineeshipVacancyProvider.FindVacancies(model);
            traineeshipSearchResponseViewModel.VacancySearch = model;
            if (traineeshipSearchResponseViewModel.ExactMatchFound)
            {
                var id = traineeshipSearchResponseViewModel.Vacancies.Single().Id;
                return GetMediatorResponse<TraineeshipSearchResponseViewModel>(TraineeshipSearchMediatorCodes.Results.ExactMatchFound, parameters: new { id });
            }

            if (traineeshipSearchResponseViewModel.VacancySearch != null)
            {
                traineeshipSearchResponseViewModel.VacancySearch.SortTypes = GetSortTypes(model.SortType);
            }

            if (traineeshipSearchResponseViewModel.HasError())
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.HasError, new TraineeshipSearchResponseViewModel { VacancySearch = model }, traineeshipSearchResponseViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            //Populate Google static maps URL
            foreach (var vacancy in traineeshipSearchResponseViewModel.Vacancies)
            {
                vacancy.GoogleStaticMapsUrl = _googleMapsProvider.GetStaticMapsUrl(vacancy.Location);
            }

            return GetMediatorResponse(TraineeshipSearchMediatorCodes.Results.Ok, traineeshipSearchResponseViewModel);
        }

        public MediatorResponse<TraineeshipVacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<TraineeshipVacancyDetailViewModel>(TraineeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            var vacancyDetailViewModel = _traineeshipVacancyProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            return GetDetails(vacancyDetailViewModel);
        }

        public MediatorResponse<TraineeshipVacancyDetailViewModel> DetailsByReferenceNumber(string vacancyReferenceNumberString, Guid? candidateId)
        {
            int vacancyReferenceNumber;
            if (VacancyHelper.TryGetVacancyReferenceNumber(vacancyReferenceNumberString, out vacancyReferenceNumber))
            {
                var vacancyDetailViewModel = _traineeshipVacancyProvider.GetVacancyDetailViewModelByReferenceNumber(candidateId, vacancyReferenceNumber);
                return GetDetails(vacancyDetailViewModel);
            }
            return GetMediatorResponse<TraineeshipVacancyDetailViewModel>(TraineeshipSearchMediatorCodes.Details.VacancyNotFound);
        }

        private MediatorResponse<TraineeshipVacancyDetailViewModel> GetDetails(TraineeshipVacancyDetailViewModel vacancyDetailViewModel)
        {
            if (vacancyDetailViewModel == null || vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable)
            {
                return GetMediatorResponse<TraineeshipVacancyDetailViewModel>(TraineeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(TraineeshipSearchMediatorCodes.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            var distance = UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);
            var lastViewedVacancy = UserDataProvider.PopLastViewedVacancy();

            if (HasToPopulateDistance(vacancyDetailViewModel.Id, distance, lastViewedVacancy))
            {
                vacancyDetailViewModel.Distance = distance;
                UserDataProvider.Push(CandidateDataItemNames.VacancyDistance, distance);
            }

            UserDataProvider.PushLastViewedVacancyId(vacancyDetailViewModel.Id, VacancyType.Traineeship);

            return GetMediatorResponse(TraineeshipSearchMediatorCodes.Details.Ok, vacancyDetailViewModel);
        }
    }
}
using SFA.Apprenticeships.Web.Common.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.Mediators.Search
{
    using Apprenticeships.Application.Interfaces;
    using Apprenticeships.Application.Interfaces.ReferenceData;
    using Apprenticeships.Application.Interfaces.Vacancies;
    using Common.Configuration;
    using Common.Constants;
    using Common.Framework;
    using Common.Providers;
    using Constants;
    using Constants.Pages;
    using Domain.Entities.Applications;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies;
    using Extensions;
    using Infrastructure.VacancySearch.Configuration;
    using Providers;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Validators;
    using ViewModels.Account;
    using ViewModels.VacancySearch;

    public class ApprenticeshipSearchMediator : SearchMediatorBase, IApprenticeshipSearchMediator
    {
        private readonly ISearchProvider _searchProvider;
        private readonly IConfigurationService _configService;
        private readonly ICandidateServiceProvider _candidateServiceProvider;
        private readonly IReferenceDataService _referenceDataService;
        private readonly ApprenticeshipSearchViewModelServerValidator _searchRequestValidator;
        private readonly ApprenticeshipSearchViewModelLocationValidator _searchLocationValidator;
        private readonly IApprenticeshipVacancyProvider _apprenticeshipVacancyProvider;
        private readonly IGoogleMapsProvider _googleMapsProvider;

        private readonly string[] _blacklistedCategoryCodes;

        public ApprenticeshipSearchMediator(
            IConfigurationService configService,
            ISearchProvider searchProvider,
            ICandidateServiceProvider candidateServiceProvider,
            IUserDataProvider userDataProvider,
            IReferenceDataService referenceDataService,
            ApprenticeshipSearchViewModelServerValidator searchRequestValidator,
            ApprenticeshipSearchViewModelLocationValidator searchLocationValidator,
            IApprenticeshipVacancyProvider apprenticeshipVacancyProvider, IGoogleMapsProvider googleMapsProvider)
            : base(configService, userDataProvider)
        {
            _configService = configService;
            _candidateServiceProvider = candidateServiceProvider;
            _searchProvider = searchProvider;
            _referenceDataService = referenceDataService;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _apprenticeshipVacancyProvider = apprenticeshipVacancyProvider;
            _googleMapsProvider = googleMapsProvider;
            _blacklistedCategoryCodes = configService.Get<CommonWebConfiguration>().BlacklistedCategoryCodes.Split(',');
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> Index(Guid? candidateId, ApprenticeshipSearchMode searchMode, bool reset)
        {
            var lastSearchedLocation = UserDataProvider.Get(UserDataItemNames.LastSearchedLocation);
            if (string.IsNullOrWhiteSpace(lastSearchedLocation) && candidateId.HasValue)
            {
                var candidate = _candidateServiceProvider.GetCandidate(candidateId.Value);
                UserDataProvider.Push(UserDataItemNames.LastSearchedLocation, lastSearchedLocation = candidate.RegistrationDetails.Address.Postcode);
            }

            if (!candidateId.HasValue && searchMode == ApprenticeshipSearchMode.SavedSearches)
            {
                searchMode = ApprenticeshipSearchMode.Keyword;
            }

            var distances = GetDistances(true);
            var sortTypes = GetSortTypes();
            var resultsPerPage = GetResultsPerPage();
            var apprenticeshipLevels = GetApprenticeshipLevels();
            var apprenticeshipLevel = GetApprenticeshipLevel(reset);
            var searchFields = GetSearchFields();
            var categories = GetCategories();
            var savedSearches = GetSavedSearches(candidateId);

            var viewModel = new ApprenticeshipSearchViewModel
            {
                WithinDistance = 5,
                LocationType = VacancyLocationType.NonNational,
                Location = SplitSearchLocation(lastSearchedLocation, 0),
                Latitude = SplitSearchLocation(lastSearchedLocation, 1).GetValueOrNull<double>(),
                Longitude = SplitSearchLocation(lastSearchedLocation, 2).GetValueOrNull<double>(),
                Distances = distances,
                SortTypes = sortTypes,
                ResultsPerPage = resultsPerPage,
                ApprenticeshipLevels = apprenticeshipLevels,
                ApprenticeshipLevel = apprenticeshipLevel,
                SearchFields = searchFields,
                Categories = categories,
                SavedSearches = savedSearches,
                SearchMode = searchMode
            };

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Index.Ok, viewModel);
        }

        private static SelectList GetApprenticeshipLevels(string selectedValue = "All")
        {
            var apprenticeshipLevels = new SelectList(
                new[]
                {
                    new {ApprenticeshipLevel = "All", Name = "All levels"},
                    new {ApprenticeshipLevel = "Intermediate", Name = "Intermediate"},
                    new {ApprenticeshipLevel = "Advanced", Name = "Advanced"},
                    new {ApprenticeshipLevel = "Higher", Name = "Higher"},
                    new {ApprenticeshipLevel = "Degree", Name = "Degree"}
                },
                "ApprenticeshipLevel",
                "Name",
                selectedValue
                );

            return apprenticeshipLevels;
        }

        protected SelectList GetSearchFields(bool addRefineOption = false, string selectedValue = "All")
        {
            var allDisplayName = addRefineOption && selectedValue == "All" ? "-- Refine search --" : "All";

            var searchFieldsOptions = new List<object>
            {
                new { FieldName = ApprenticeshipSearchField.All.ToString(), DisplayName = allDisplayName},
                new { FieldName = ApprenticeshipSearchField.JobTitle.ToString(), DisplayName = "Job title"},
                new { FieldName = ApprenticeshipSearchField.Description.ToString(), DisplayName = "Description"},
                new { FieldName = ApprenticeshipSearchField.Employer.ToString(), DisplayName = "Employer"},
                new { FieldName = ApprenticeshipSearchField.ReferenceNumber.ToString(), DisplayName = "Ref. number"},
            };

            var searchFactors = _configService.Get<SearchFactorConfiguration>();

            if (searchFactors != null && searchFactors.ProviderFactors != null && searchFactors.ProviderFactors.Enabled)
            {
                searchFieldsOptions.Insert(4, new { FieldName = ApprenticeshipSearchField.Provider.ToString(), DisplayName = "Training Provider" });
            }

            var searchFields = new SelectList(
                searchFieldsOptions.ToArray(),
                "FieldName",
                "DisplayName"
                );

            return searchFields;
        }

        private string GetApprenticeshipLevel(bool reset)
        {
            if (reset)
            {
                UserDataProvider.Pop(CandidateDataItemNames.ApprenticeshipLevel);
            }

            return UserDataProvider.Get(CandidateDataItemNames.ApprenticeshipLevel) ?? "All";
        }

        private List<Category> GetCategories()
        {
            var cats = _referenceDataService.GetCategories();
            return cats == null ? null : _referenceDataService.GetCategories().Where(c => !_blacklistedCategoryCodes.Contains(c.CodeName)).ToList();
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> SearchValidation(Guid? candidateId, ApprenticeshipSearchViewModel model)
        {
            if (!candidateId.HasValue && model.SearchMode == ApprenticeshipSearchMode.SavedSearches)
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SearchValidation.CandidateNotLoggedIn, model);
            }

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                model.Distances = GetDistances(true);
                model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);
                model.ApprenticeshipLevels = GetApprenticeshipLevels(model.ApprenticeshipLevel);
                model.SearchFields = GetSearchFields(false, model.SearchField);
                model.Categories = GetCategories();
                model.SavedSearches = GetSavedSearches(candidateId);

                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SearchValidation.ValidationError, model, clientResult);
            }

            var code = model.SearchMode == ApprenticeshipSearchMode.SavedSearches
                ? ApprenticeshipSearchMediatorCodes.SearchValidation.RunSavedSearch
                : ApprenticeshipSearchMediatorCodes.SearchValidation.Ok;

            return GetMediatorResponse(code, model);
        }

        public MediatorResponse<ApprenticeshipSearchResponseViewModel> Results(Guid? candidateId, ApprenticeshipSearchViewModel model)
        {
            UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);

            NormalizeCategoriesAndSubcategories(model);

            if (model.ResultsPerPage == 0)
            {
                model.ResultsPerPage = GetResultsPerPage();
            }

            if (string.IsNullOrEmpty(model.ApprenticeshipLevel))
            {
                model.ApprenticeshipLevel = GetApprenticeshipLevel(false);
            }

            UserDataProvider.Push(UserDataItemNames.ResultsPerPage, model.ResultsPerPage.ToString(CultureInfo.InvariantCulture));
            UserDataProvider.Push(CandidateDataItemNames.ApprenticeshipLevel, model.ApprenticeshipLevel.ToString(CultureInfo.InvariantCulture));

            if (model.SearchAction == SearchAction.Search && model.LocationType != VacancyLocationType.NonNational)
            {
                model.LocationType = VacancyLocationType.NonNational;
            }

            PopulateSortType(model);

            model.Distances = GetDistances(true);
            model.ResultsPerPageSelectList = GetResultsPerPageSelectList(model.ResultsPerPage);
            model.ApprenticeshipLevels = GetApprenticeshipLevels(model.ApprenticeshipLevel);
            model.SearchFields = GetSearchFields(true, model.SearchField);
            model.Categories = GetCategories();

            var clientResult = _searchRequestValidator.Validate(model);

            if (!clientResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.ValidationError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, clientResult);
            }

            if (!HasGeoPoint(model) && !string.IsNullOrEmpty(model.Location))
            {
                // User did not select a location from the dropdown list, provide suggested locations.
                var suggestedLocations = _searchProvider.FindLocation(model.Location.Trim());

                if (suggestedLocations.HasError())
                {
                    return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, suggestedLocations.ViewModelMessage, UserMessageLevel.Warning);
                }

                if (suggestedLocations.Locations.Any())
                {
                    var location = suggestedLocations.Locations.First();

                    model.Location = location.Name;
                    model.Latitude = location.Latitude;
                    model.Longitude = location.Longitude;
                    model.Hash = model.LatLonLocHash();

                    model.LocationSearches = suggestedLocations.Locations.Skip(1).Select(each =>
                    {
                        var vsvm = new ApprenticeshipSearchViewModel
                        {
                            Keywords = model.Keywords,
                            Location = each.Name,
                            Latitude = each.Latitude,
                            Longitude = each.Longitude,
                            PageNumber = model.PageNumber,
                            SortType = model.SortType,
                            WithinDistance = model.WithinDistance,
                            ResultsPerPage = model.ResultsPerPage,
                            Category = model.Category,
                            SearchFields = model.SearchFields,
                            SubCategories = model.SubCategories,
                            SearchMode = model.SearchMode
                        };

                        vsvm.Hash = vsvm.LatLonLocHash();

                        return vsvm;
                    }).ToArray();
                }
            }

            var locationResult = _searchLocationValidator.Validate(model);

            if (!locationResult.IsValid)
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.Ok, new ApprenticeshipSearchResponseViewModel { VacancySearch = model });
            }

            UserDataProvider.Push(UserDataItemNames.LastSearchedLocation, string.Join("|", model.Location, model.Latitude, model.Longitude));

            RemoveInvalidSubCategories(model);

            var searchModel = GetSearchModel(model);

            PopulateSortType(searchModel);

            model.SortType = searchModel.SortType;

            var results = _apprenticeshipVacancyProvider.FindVacancies(searchModel);

            if (results.VacancySearch != null)
            {
                model.SortType = results.VacancySearch.SortType;
                model.LocationType = results.VacancySearch.LocationType;
            }

            results.VacancySearch = model;

            if (results.HasError())
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.HasError, new ApprenticeshipSearchResponseViewModel { VacancySearch = model }, results.ViewModelMessage, UserMessageLevel.Warning);
            }

            if (results.ExactMatchFound)
            {
                var id = results.Vacancies.Single().Id;
                return GetMediatorResponse<ApprenticeshipSearchResponseViewModel>(ApprenticeshipSearchMediatorCodes.Results.ExactMatchFound, parameters: new { id });
            }

            if (model.SearchAction == SearchAction.Search && results.TotalLocalHits > 0)
            {
                results.VacancySearch.LocationType = VacancyLocationType.NonNational;
            }

            var isLocalLocationType = results.VacancySearch.LocationType != VacancyLocationType.National;

            results.VacancySearch.SortTypes = GetSortTypes(model.SortType, model.Keywords, isLocalLocationType);

            if (candidateId.HasValue)
            {
                SetSavedVacancyStatuses(candidateId.Value, results);
            }

            //Populate Google static maps URL
            foreach (var vacancy in results.Vacancies)
            {
                vacancy.GoogleStaticMapsUrl = _googleMapsProvider.GetStaticMapsUrl(vacancy.Location);
            }

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Results.Ok, results);
        }

        private void NormalizeCategoriesAndSubcategories(ApprenticeshipSearchViewModel model)
        {
            if (!CategoryPrefixes.IsSectorSubjectAreaTier1Code(model.Category))
            {
                model.Category = CategoryPrefixes.GetSectorSubjectAreaTier1Code(model.Category);
            }

            model.SubCategories = model.SubCategories?.Select(c =>
            {
                if (!CategoryPrefixes.IsFrameworkCode(c) && !CategoryPrefixes.IsStandardSectorCode(c))
                {
                    // Is an old saved search, so it's a framework
                    return CategoryPrefixes.GetFrameworkCode(c);
                }

                return c;
            }).ToArray();
        }

        public MediatorResponse<ApprenticeshipSearchViewModel> SaveSearch(Guid candidateId, ApprenticeshipSearchViewModel viewModel)
        {
            viewModel.Categories = GetCategories();

            if (!viewModel.Longitude.HasValue || !viewModel.Latitude.HasValue)
            {
                //todo: should we fail to save the search if we don't have a location?
                //todo: or we could try work out the geo location from the location name, that way we can filter
                //todo: searches without geo loctions from any saved search emailing?
            }

            viewModel = _candidateServiceProvider.CreateSavedSearch(candidateId, viewModel);

            if (viewModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SaveSearch.HasError, viewModel, viewModel.ViewModelMessage, UserMessageLevel.Error);
            }

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SaveSearch.Ok, viewModel, VacancySearchResultsPageMessages.SaveSearchSuccess, UserMessageLevel.Success);
        }

        public MediatorResponse<ApprenticeshipVacancyDetailViewModel> Details(string vacancyIdString, Guid? candidateId)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<ApprenticeshipVacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            var vacancyDetailViewModel = _apprenticeshipVacancyProvider.GetVacancyDetailViewModel(candidateId, vacancyId);

            return GetDetails(vacancyDetailViewModel);
        }

        public MediatorResponse<ApprenticeshipVacancyDetailViewModel> DetailsByReferenceNumber(string vacancyReferenceNumberString, Guid? candidateId)
        {
            int vacancyReferenceNumber;
            if (VacancyHelper.TryGetVacancyReferenceNumber(vacancyReferenceNumberString, out vacancyReferenceNumber))
            {
                var vacancyDetailViewModel = _apprenticeshipVacancyProvider.GetVacancyDetailViewModelByReferenceNumber(candidateId, vacancyReferenceNumber);
                return GetDetails(vacancyDetailViewModel);
            }
            return GetMediatorResponse<ApprenticeshipVacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
        }

        private MediatorResponse<ApprenticeshipVacancyDetailViewModel> GetDetails(ApprenticeshipVacancyDetailViewModel vacancyDetailViewModel)
        {
            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<ApprenticeshipVacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Details.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }
            if (vacancyDetailViewModel.CandidateApplicationStatus == ApplicationStatuses.Draft &&
                vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Expired)
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Details.VacancyExpired, vacancyDetailViewModel);
            }
            if ((!vacancyDetailViewModel.CandidateApplicationStatus.HasValue && vacancyDetailViewModel.VacancyStatus != VacancyStatuses.Live) ||
                (vacancyDetailViewModel.CandidateApplicationStatus.HasValue && vacancyDetailViewModel.VacancyStatus == VacancyStatuses.Unavailable))
            {
                // Candidate has no application for the vacancy and the vacancy is no longer live OR
                // candidate has an application (at least a draft) but the vacancy is no longer available.
                return GetMediatorResponse<ApprenticeshipVacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.Details.VacancyNotFound);
            }

            var distance = UserDataProvider.Pop(CandidateDataItemNames.VacancyDistance);
            var lastViewedVacancy = UserDataProvider.PopLastViewedVacancy();

            if (HasToPopulateDistance(vacancyDetailViewModel.Id, distance, lastViewedVacancy))
            {
                vacancyDetailViewModel.Distance = distance;
                UserDataProvider.Push(CandidateDataItemNames.VacancyDistance, distance);
            }

            UserDataProvider.PushLastViewedVacancyId(vacancyDetailViewModel.Id, VacancyType.Apprenticeship);

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.Details.Ok, vacancyDetailViewModel);
        }

        public MediatorResponse<ApprenticeshipVacancyDetailViewModel> RedirectToExternalWebsite(string vacancyIdString)
        {
            int vacancyId;

            if (!TryParseVacancyId(vacancyIdString, out vacancyId))
            {
                return GetMediatorResponse<ApprenticeshipVacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.VacancyNotFound);
            }

            var vacancyDetailViewModel = _apprenticeshipVacancyProvider.IncrementClickThroughFor(vacancyId);

            if (vacancyDetailViewModel == null)
            {
                return GetMediatorResponse<ApprenticeshipVacancyDetailViewModel>(ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.VacancyNotFound);
            }

            if (vacancyDetailViewModel.HasError())
            {
                return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.VacancyHasError, vacancyDetailViewModel, vacancyDetailViewModel.ViewModelMessage, UserMessageLevel.Warning);
            }

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.RedirectToExternalWebsite.Ok, vacancyDetailViewModel);
        }

        public MediatorResponse<SavedSearchViewModel> RunSavedSearch(Guid candidateId, ApprenticeshipSearchViewModel apprenticeshipSearchViewModel)
        {
            Guid savedSearchId;

            var validSavedSearchId = Guid.TryParse(apprenticeshipSearchViewModel.SavedSearchId, out savedSearchId);
            var savedSearchViewModel = validSavedSearchId
                ? _candidateServiceProvider.GetSavedSearch(candidateId, savedSearchId)
                : null;

            if (savedSearchViewModel == null)
            {
                return GetMediatorResponse(
                    ApprenticeshipSearchMediatorCodes.SavedSearch.SavedSearchNotFound,
                    default(SavedSearchViewModel),
                    ApprenticeshipsSearchPageMessages.SavedSearchNotFound,
                    UserMessageLevel.Error);
            }

            if (savedSearchViewModel.HasError())
            {
                return GetMediatorResponse(
                    ApprenticeshipSearchMediatorCodes.SavedSearch.RunSaveSearchFailed,
                    savedSearchViewModel,
                    ApprenticeshipsSearchPageMessages.RunSavedSearchFailed,
                    UserMessageLevel.Error);
            }

            return GetMediatorResponse(ApprenticeshipSearchMediatorCodes.SavedSearch.Ok, savedSearchViewModel);
        }



        #region Helpers

        private static ApprenticeshipSearchViewModel GetSearchModel(ApprenticeshipSearchViewModel model)
        {
            //Create a new search view model based on the search mode and user data
            var searchModel = new ApprenticeshipSearchViewModel(model);

            switch (searchModel.SearchMode)
            {
                case ApprenticeshipSearchMode.Keyword:
                    searchModel.Category = null;
                    searchModel.SubCategories = null;
                    break;

                case ApprenticeshipSearchMode.Category:
                    searchModel.Keywords = null;
                    searchModel.Categories = model.Categories;
                    break;

                case ApprenticeshipSearchMode.SavedSearches:
                    searchModel.Keywords = null;
                    searchModel.Category = null;
                    searchModel.SubCategories = null;
                    break;
            }

            return searchModel;
        }

        private static void RemoveInvalidSubCategories(ApprenticeshipSearchViewModel model)
        {
            if (string.IsNullOrEmpty(model.Category) || model.SubCategories == null || model.Categories == null) return;
            var selectedCategory = model.Categories.SingleOrDefault(c => c.CodeName == model.Category);
            if (selectedCategory == null) return;
            model.SubCategories = model.SubCategories.Where(c => selectedCategory.SubCategories.Any(sc => sc.CodeName == c)).ToArray();
        }

        //TODO: Tell don't ask?
        private static void PopulateSortType(ApprenticeshipSearchViewModel model)
        {
            if (model.LocationType == VacancyLocationType.NonNational && model.SortType == VacancySearchSortType.Relevancy &&
                string.IsNullOrWhiteSpace(model.Keywords))
            {
                model.SortType = VacancySearchSortType.Distance;
            }

            if (model.LocationType == VacancyLocationType.National && string.IsNullOrWhiteSpace(model.Keywords) &&
                model.SortType != VacancySearchSortType.ClosingDate && model.SearchAction != SearchAction.Sort)
            {
                model.SortType = VacancySearchSortType.ClosingDate;
            }

            if (model.SearchAction == SearchAction.Search && !string.IsNullOrWhiteSpace(model.Keywords))
            {
                model.SortType = VacancySearchSortType.Relevancy;
            }

            if (model.SearchAction == SearchAction.LocationTypeChanged)
            {
                if (!string.IsNullOrWhiteSpace(model.Keywords))
                {
                    model.SortType = VacancySearchSortType.Relevancy;
                }
                else if (model.LocationType == VacancyLocationType.National)
                {
                    // TODO: DEADCODE: added by vgaltes back in Feb.
                    //if (model.SortType != VacancySearchSortType.RecentlyAdded)
                    //{
                    model.SortType = VacancySearchSortType.ClosingDate;
                    //}
                }
                else
                {
                    model.SortType = VacancySearchSortType.Distance;
                }
            }
        }

        private void SetSavedVacancyStatuses(Guid candidateId, ApprenticeshipSearchResponseViewModel results)
        {
            var apprenticeshipApplications = _candidateServiceProvider.GetApprenticeshipApplications(candidateId, false).ToList();

            foreach (var result in results.Vacancies)
            {
                var apprenticeshipApplication = apprenticeshipApplications
                    .SingleOrDefault(each => each.LegacyVacancyId == result.Id);

                result.CandidateApplicationStatus = apprenticeshipApplication == null
                    ? default(ApplicationStatuses?)
                    : apprenticeshipApplication.Status;
            }
        }

        private SavedSearchViewModel[] GetSavedSearches(Guid? candidateId)
        {
            if (candidateId == null)
            {
                return null;
            }

            var savedSearches = _candidateServiceProvider.GetSavedSearches(candidateId.Value);

            return savedSearches == null
                ? null
                : savedSearches.OrderByDescending(each => each.DateCreated).ToArray();
        }

        #endregion
    }
}
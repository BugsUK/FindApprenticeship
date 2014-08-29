﻿namespace SFA.Apprenticeships.Web.Candidate.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using ActionResults;
    using Application.Interfaces.Vacancies;
    using Common.Constants;
    using Domain.Interfaces.Configuration;
    using FluentValidation.Mvc;
    using Microsoft.Ajax.Utilities;
    using Providers;
    using Validators;
    using ViewModels.VacancySearch;

    public class VacancySearchController : CandidateControllerBase //todo: rename
    {
        private readonly ISearchProvider _searchProvider;
        private readonly VacancySearchViewModelClientValidator _searchRequestValidator;
        private readonly VacancySearchViewModelLocationValidator _searchLocationValidator;
        private readonly IVacancyDetailProvider _vacancyDetailProvider;
        private readonly int _vacancyResultsPerPage;

        public VacancySearchController(IConfigurationManager configManager,
            ISearchProvider searchProvider,
            VacancySearchViewModelClientValidator searchRequestValidator,
            VacancySearchViewModelLocationValidator searchLocationValidator,
            IVacancyDetailProvider vacancyDetailProvider)
        {
            _searchProvider = searchProvider;
            _searchRequestValidator = searchRequestValidator;
            _searchLocationValidator = searchLocationValidator;
            _vacancyDetailProvider = vacancyDetailProvider;
            _vacancyResultsPerPage = configManager.GetAppSetting<int>("VacancyResultsPerPage");
        }

        [HttpGet]
        public ActionResult Index()
        {
            PopulateDistances();
            PopulateSortType();

            return View(new VacancySearchViewModel { WithinDistance = 2 });
        }

        [HttpGet]
        public ActionResult Search(VacancySearchResponseViewModel searchViewModel)
        {
            return RedirectToAction("results", searchViewModel.VacancySearch);
        }

        [HttpGet]
        public ActionResult Results(VacancySearchViewModel searchViewModel)
        {
            UserData.Pop(UserDataItemNames.VacancyDistance);

            PopulateLookups(searchViewModel);

            var clientResult = _searchRequestValidator.Validate(searchViewModel);

            if (!clientResult.IsValid)
            {
                ModelState.Clear();
                clientResult.AddToModelState(ModelState, string.Empty);

                return View("results", new VacancySearchResponseViewModel { VacancySearch = searchViewModel });
            }

            searchViewModel.CheckLatLonLocHash();

            if (!HasGeoPoint(searchViewModel))
            {
                // Either user not selected item from dropdown or javascript disabled.
                var suggestedLocations = FindSuggestedLocations(searchViewModel);

                if (suggestedLocations != null)
                {
                    ViewBag.LocationSearches = suggestedLocations;
                }
            }

            var locationResult = _searchLocationValidator.Validate(searchViewModel);

            if (!locationResult.IsValid)
            {
                ModelState.Clear();
                return View("results", new VacancySearchResponseViewModel { VacancySearch = searchViewModel });
            }

            var results = _searchProvider.FindVacancies(searchViewModel, _vacancyResultsPerPage);

            return View("results", results);
        }

        [HttpGet]
        public ActionResult DetailsWithDistance(int id, string distance)
        {
            UserData.Push(UserDataItemNames.VacancyDistance, distance.ToString(CultureInfo.InvariantCulture));

            return RedirectToAction("Details", new { id });
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Guid? candidateId = null;

            if (Request.IsAuthenticated)
            {
                candidateId = UserContext.CandidateId;
            }

            var vacancy = _vacancyDetailProvider.GetVacancyDetailViewModel(candidateId, id);

            if (vacancy == null)
            {
                return new VacancyNotFoundResult();
            }

            var distance = UserData.Pop(UserDataItemNames.VacancyDistance);
            var lastVacancyId = UserData.Pop(UserDataItemNames.LastViewedVacancyId);

            if (!string.IsNullOrWhiteSpace(distance)
                    && !string.IsNullOrWhiteSpace(lastVacancyId)
                    && int.Parse(lastVacancyId) == id)
            {
                ViewBag.Distance = distance;
                UserData.Push(UserDataItemNames.VacancyDistance, distance);
            }

            UserData.Push(UserDataItemNames.LastViewedVacancyId, id.ToStringInvariant());

            return View(vacancy);
        }

        #region Helpers

        private static bool HasGeoPoint(VacancySearchViewModel searchViewModel)
        {
            return searchViewModel.Latitude.HasValue && searchViewModel.Longitude.HasValue;
        }

        private void PopulateLookups(VacancySearchViewModel searchViewModel)
        {
            PopulateDistances(searchViewModel.WithinDistance);
            PopulateSortType(searchViewModel.SortType);
        }

        private VacancySearchViewModel[] FindSuggestedLocations(VacancySearchViewModel searchViewModel)
        {
            var locations = _searchProvider.FindLocation(searchViewModel.Location.Trim()).ToList();

            if (!locations.Any())
            {
                return null;
            }

            var location = locations.First();

            searchViewModel.Location = location.Name;
            searchViewModel.Latitude = location.Latitude;
            searchViewModel.Longitude = location.Longitude;

            return locations.Skip(1).Select(l =>
            {
                var vsvm = new VacancySearchViewModel
                {
                    Keywords = searchViewModel.Keywords,
                    Location = l.Name,
                    Latitude = l.Latitude,
                    Longitude = l.Longitude,
                    PageNumber = searchViewModel.PageNumber,
                    SortType = searchViewModel.SortType,
                    WithinDistance = searchViewModel.WithinDistance
                };

                vsvm.Hash = vsvm.LatLonLocHash();

                return vsvm;
            }).ToArray();
        }

        private void PopulateDistances(int selectedValue = 2)
        {
            var distances = new SelectList(
                new[]
                {
                    new {WithinDistance = 2, Name = "This area only"},
                    new {WithinDistance = 5, Name = "5 miles"},
                    new {WithinDistance = 10, Name = "10 miles"},
                    new {WithinDistance = 15, Name = "15 miles"},
                    new {WithinDistance = 20, Name = "20 miles"},
                    new {WithinDistance = 30, Name = "30 miles"},
                    new {WithinDistance = 40, Name = "40 miles"}
                },
                "WithinDistance",
                "Name",
                selectedValue
                );

            ViewBag.Distances = distances;
        }

        private void PopulateSortType(VacancySortType selectedSortType = VacancySortType.Distance)
        {
            var sortTypes = new SelectList(
                new[]
                {
                    new {SortType = VacancySortType.Distance, Name = "Distance"},
                    new {SortType = VacancySortType.ClosingDate, Name = "Closing Date"}
                    //new { SortType = VacancySortType.Relevancy, Name = "Best Match" }
                },
                "SortType",
                "Name",
                selectedSortType
                );

            ViewBag.SortTypes = sortTypes;
        }

        #endregion
    }
}

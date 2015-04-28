namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web;
    using System.Web.Mvc;
    using Account;
    using Application.Interfaces.Vacancies;
    using Common.Framework;
    using Constants.ViewModels;
    using Domain.Entities.ReferenceData;
    using Domain.Entities.Vacancies.Apprenticeships;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof (ApprenticeshipSearchViewModelClientValidator))]
    public class ApprenticeshipSearchViewModel : VacancySearchViewModel
    {

        private string _searchField;

        public ApprenticeshipSearchViewModel()
        {
            DisplaySubCategory = true;
            DisplayDescription = true;
            DisplayDistance = true;
            DisplayClosingDate = true;
            DisplayStartDate = true;
        }

        public ApprenticeshipSearchViewModel(ApprenticeshipSearchViewModel viewModel) : base (viewModel)
        {
            Keywords = viewModel.Keywords;
            LocationType = viewModel.LocationType;
            ApprenticeshipLevel = viewModel.ApprenticeshipLevel;
            Category = viewModel.Category;
            SubCategories = viewModel.SubCategories;
            SearchMode = viewModel.SearchMode;
            SearchField = viewModel.SearchField;
            SavedSearches = viewModel.SavedSearches;
            DisplaySubCategory = DisplaySubCategory;
            DisplayDescription = DisplayDescription;
            DisplayDistance = DisplayDistance;
            DisplayClosingDate = DisplayClosingDate;
            DisplayStartDate = DisplayStartDate;
            DisplayApprenticeshipLevel = DisplayApprenticeshipLevel;
            DisplayWage = DisplayWage;
        }

        [Display(Name = ApprenticeshipSearchViewModelMessages.KeywordMessages.LabelText, Description = ApprenticeshipSearchViewModelMessages.KeywordMessages.HintText)]
        public string Keywords { get; set; }

        [Display(Name = ApprenticeshipSearchViewModelMessages.LocationMessages.LabelText, Description = ApprenticeshipSearchViewModelMessages.LocationMessages.HintText)]
        public override string Location { get; set; }

        public ApprenticeshipLocationType LocationType { get; set; }

        public ApprenticeshipSearchViewModel[] LocationSearches { get; set; }

        public SelectList ApprenticeshipLevels { get; set; }

        public string ApprenticeshipLevel { get; set; }

        public SelectList SearchFields { get; set; }

        public string SearchField
        {
            get
            {
                return string.IsNullOrEmpty(_searchField) ? ApprenticeshipSearchField.All.ToString() : _searchField;
            }
            set
            {
                ApprenticeshipSearchField searchField;
                if (Enum.TryParse(value, out searchField))
                {
                    _searchField = value;
                }
                else
                {
                    _searchField = ApprenticeshipSearchField.All.ToString();
                }
            }
        }

        public object RouteValues
        {
            get
            {
                return new
                {
                    ApprenticeshipLevel,
                    Category,
                    SubCategories = SubCategories == null || SubCategories.Length == 0 ? null : SubCategories,
                    Hash,
                    Keywords,
                    Latitude,
                    Longitude,
                    Location,
                    LocationType,
                    PageNumber,
                    ResultsPerPage,
                    SearchAction,
                    SearchField,
                    SearchMode,
                    SortType,
                    WithinDistance
                };
            }
        }

        public IList<Category> Categories { get; set; }

        public string Category { get; set; }

        public string[] SubCategories { get; set; }

        public ApprenticeshipSearchMode SearchMode { get; set; }

        public SavedSearchViewModel[] SavedSearches { get; set; }

        public string SavedSearchId { get; set; }

        public bool DisplaySubCategory { get; set; }

        public bool DisplayDescription { get; set; }

        public bool DisplayDistance { get; set; }

        public bool DisplayClosingDate { get; set; }

        public bool DisplayStartDate { get; set; }

        public bool DisplayApprenticeshipLevel { get; set; }

        public bool DisplayWage { get; set; }

        #region Helpers

        public static ApprenticeshipSearchViewModel FromSearchUrl(string searchUrl)
        {
            var searchUri = new Uri(new Uri("http://base"), searchUrl);
            var queryStringParams = HttpUtility.ParseQueryString(searchUri.Query);

            var searchModel = new ApprenticeshipSearchViewModel();

            searchModel.SearchMode = queryStringParams.Get("SearchMode") != null
                ? (ApprenticeshipSearchMode)Enum.Parse(typeof(ApprenticeshipSearchMode), queryStringParams.Get("SearchMode"))
                : ApprenticeshipSearchMode.Keyword;

            searchModel.SortType = queryStringParams.Get("SortType") != null
                ? (VacancySearchSortType)Enum.Parse(typeof(VacancySearchSortType), queryStringParams.Get("SortType"))
                : VacancySearchSortType.Relevancy;

            searchModel.LocationType = queryStringParams.Get("LocationType") != null
                ? (ApprenticeshipLocationType)Enum.Parse(typeof(ApprenticeshipLocationType), queryStringParams.Get("LocationType"))
                : ApprenticeshipLocationType.NonNational;

            searchModel.Latitude = queryStringParams.Get("Latitude").GetValueOrNull<double>();
            searchModel.Longitude = queryStringParams.Get("Longitude").GetValueOrNull<double>();
            searchModel.WithinDistance = queryStringParams.Get("WithinDistance").GetValueOrDefault<int>();
            searchModel.ResultsPerPage = queryStringParams.Get("ResultsPerPage").GetValueOrDefault<int>();

            searchModel.Keywords = queryStringParams.Get("Keywords");
            searchModel.Location = queryStringParams.Get("Location");
            searchModel.ApprenticeshipLevel = queryStringParams.Get("ApprenticeshipLevel");
            searchModel.SearchField = queryStringParams.Get("SearchField");
            searchModel.Category = queryStringParams.Get("Category");
            searchModel.SubCategories = queryStringParams.Get("SubCategories") != null ? queryStringParams.Get("SubCategories").Split(',') : null;

            return searchModel;
        }

        #endregion
    }
}

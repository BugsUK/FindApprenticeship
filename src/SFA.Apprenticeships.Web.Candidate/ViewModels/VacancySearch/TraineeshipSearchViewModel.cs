namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using Application.Interfaces.Vacancies;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Validators;

    [Validator(typeof(TraineeshipSearchViewModelClientValidator))]
    public class TraineeshipSearchViewModel : VacancySearchViewModel
    {
        public TraineeshipSearchViewModel()
        {
            SortType = VacancySearchSortType.Distance;
            LocationSearches = Enumerable.Empty<TraineeshipSearchViewModel>();
        }

        public TraineeshipSearchViewModel(TraineeshipSearchViewModel viewModel) : base(viewModel)
        {
            LocationSearches = Enumerable.Empty<TraineeshipSearchViewModel>();
        }

        [Display(Name = TraineeshipSearchViewModelMessages.LocationMessages.LabelText, Description = TraineeshipSearchViewModelMessages.LocationMessages.HintText)]
        public override string Location { get; set; }

        [Display(Name = TraineeshipSearchViewModelMessages.ReferenceNumberMessages.LabelText)]
        public string ReferenceNumber { get; set; }

        public IEnumerable<TraineeshipSearchViewModel> LocationSearches { get; set; }

        public object RouteValues
        {
            get
            {
                return new
                {
                    Hash,
                    Latitude,
                    Longitude,
                    Location,
                    PageNumber,
                    ResultsPerPage,
                    SearchAction,
                    SortType,
                    WithinDistance
                };
            }
        }
    }
}

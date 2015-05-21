namespace SFA.Apprenticeships.Web.Candidate.ViewModels.VacancySearch
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Application.Interfaces.Vacancies;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
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

        public IEnumerable<TraineeshipSearchViewModel> LocationSearches { get; set; }
    }
}

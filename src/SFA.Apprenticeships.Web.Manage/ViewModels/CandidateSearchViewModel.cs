namespace SFA.Apprenticeships.Web.Manage.ViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Constants.ViewModels;
    using FluentValidation.Attributes;
    using Validators;

    [Validator(typeof(CandidateSearchViewModelClientValidator))]
    public class CandidateSearchViewModel
    {
        public CandidateSearchViewModel()
        {
            PageSize = 10;
            CurrentPage = 1;
        }

        private CandidateSearchViewModel(CandidateSearchViewModel viewModel) : this()
        {
            FirstName = viewModel.FirstName;
            LastName = viewModel.LastName;
            DateOfBirth = viewModel.DateOfBirth;
            Postcode = viewModel.Postcode;
            PageSize = viewModel.PageSize;
        }

        public CandidateSearchViewModel(CandidateSearchViewModel viewModel, int currentPage) : this(viewModel)
        {
            CurrentPage = currentPage;
        }

        [Display(Name = CandidateSearchViewModelMessages.FirstName.LabelText)]
        public string FirstName { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.LastName.LabelText)]
        public string LastName { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.DateOfBirth.LabelText, Description = CandidateSearchViewModelMessages.DateOfBirth.HintText)]
        public string DateOfBirth { get; set; }
        [Display(Name = CandidateSearchViewModelMessages.Postcode.LabelText, Description = CandidateSearchViewModelMessages.Postcode.HintText)]
        public string Postcode { get; set; }
        public int PageSize { get; set; }
        public List<SelectListItem> PageSizes { get; set; }
        public int CurrentPage { get; set; }
    }
}
namespace SFA.Apprenticeships.Web.Manage.Validators
{
    using FluentValidation;
    using ViewModels;

    public class CandidateSearchResultsViewModelClientValidator : AbstractValidator<CandidateSearchResultsViewModel>
    {
        public CandidateSearchResultsViewModelClientValidator()
        {
            RuleFor(x => x.SearchViewModel).SetValidator(new CandidateSearchViewModelClientValidator());
        }
    }

    public class CandidateSearchResultsViewModelServerValidator : AbstractValidator<CandidateSearchResultsViewModel>
    {
        public CandidateSearchResultsViewModelServerValidator()
        {
            RuleFor(x => x.SearchViewModel).SetValidator(new CandidateSearchViewModelServerValidator());
        }
    }
}
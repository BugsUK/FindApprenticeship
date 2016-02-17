namespace SFA.Apprenticeships.Web.Manage.Validators
{
    using Constants.ViewModels;
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

            RuleFor(x => x.SearchViewModel)
                .NotEmpty()
                .WithMessage(CandidateSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}
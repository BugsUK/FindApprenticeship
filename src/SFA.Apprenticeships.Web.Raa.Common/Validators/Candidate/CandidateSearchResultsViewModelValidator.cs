namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Candidate
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

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
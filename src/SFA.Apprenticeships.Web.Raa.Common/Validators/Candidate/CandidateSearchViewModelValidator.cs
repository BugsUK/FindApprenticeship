namespace SFA.Apprenticeships.Web.Raa.Common.Validators.Candidate
{
    using Constants.ViewModels;
    using FluentValidation;
    using ViewModels.Candidate;

    public class CandidateSearchViewModelClientValidator : AbstractValidator<CandidateSearchViewModel>
    {
        public CandidateSearchViewModelClientValidator()
        {
            this.AddCommonRules();
            this.AddClientRules();
        }
    }

    public class CandidateSearchViewModelServerValidator : AbstractValidator<CandidateSearchViewModel>
    {
        public CandidateSearchViewModelServerValidator()
        {
            this.AddCommonRules();
            this.AddServerRules();
        }
    }

    internal static class CandidateSearchViewModelValidatorRules
    {
        internal static void AddCommonRules(this AbstractValidator<CandidateSearchViewModel> validator)
        {
            validator.RuleFor(x => x.ApplicantId)
                .Matches(CandidateSearchViewModelMessages.ApplicantId.WhiteListRegularExpression)
                .WithMessage(CandidateSearchViewModelMessages.ApplicantId.WhiteListErrorText);

            validator.RuleFor(x => x.FirstName)
                .Matches(CandidateSearchViewModelMessages.FirstName.WhiteListRegularExpression)
                .WithMessage(CandidateSearchViewModelMessages.FirstName.WhiteListErrorText);

            validator.RuleFor(x => x.LastName)
                .Matches(CandidateSearchViewModelMessages.LastName.WhiteListRegularExpression)
                .WithMessage(CandidateSearchViewModelMessages.LastName.WhiteListErrorText);

            validator.RuleFor(x => x.DateOfBirth)
                .Matches(CandidateSearchViewModelMessages.DateOfBirth.WhiteListRegularExpression)
                .WithMessage(CandidateSearchViewModelMessages.DateOfBirth.WhiteListErrorText);

            validator.RuleFor(x => x.Postcode)
                .Matches(CandidateSearchViewModelMessages.Postcode.WhiteListRegularExpression)
                .WithMessage(CandidateSearchViewModelMessages.Postcode.WhiteListErrorText);
        }

        internal static void AddClientRules(this AbstractValidator<CandidateSearchViewModel> validator)
        {

        }

        internal static void AddServerRules(this AbstractValidator<CandidateSearchViewModel> validator)
        {
            validator.RuleFor(x => x)
                .Must(x => !string.IsNullOrEmpty(x.ApplicantId) || !string.IsNullOrEmpty(x.FirstName) || !string.IsNullOrEmpty(x.LastName) || !string.IsNullOrEmpty(x.DateOfBirth) || !string.IsNullOrEmpty(x.Postcode))
                .WithMessage(CandidateSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}
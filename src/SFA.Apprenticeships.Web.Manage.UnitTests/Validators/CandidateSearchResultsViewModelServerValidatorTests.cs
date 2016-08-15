namespace SFA.Apprenticeships.Web.Manage.UnitTests.Validators
{
    using Constants.ViewModels;
    using FluentAssertions;
    using Manage.Validators;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    [Parallelizable]
    public class CandidateSearchResultsViewModelServerValidatorTests
    {
        private CandidateSearchResultsViewModelServerValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CandidateSearchResultsViewModelServerValidator();
        }

        [TestCase(null, null, null, null, false)]
        [TestCase("Barry", null, null, null, true)]
        [TestCase(null, "Scott", null, null, true)]
        [TestCase(null, null, "12/02/1985", null, true)]
        [TestCase(null, null, null, "CV1 2WT", true)]
        public void AtLeastOneSearchCriteriaRequired(string firstName, string lastName, string dateOfBirth, string postcode, bool expectValid)
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel
                {
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    Postcode = postcode
                }
            };

            var result = _validator.Validate(viewModel);

            if (expectValid)
            {
                result.IsValid.Should().BeTrue();
            }
            else
            {
                result.IsValid.Should().BeFalse();
                result.Errors.Count.Should().Be(1);
                result.Errors[0].ErrorMessage.Should().Be(CandidateSearchViewModelMessages.NoSearchCriteriaErrorText);
            }
        }

        [Test]
        public void SearchViewModelCriteriaRequired()
        {
            var viewModel = new CandidateSearchResultsViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeFalse();
            result.Errors.Count.Should().Be(1);
            result.Errors[0].ErrorMessage.Should().Be(CandidateSearchViewModelMessages.NoSearchCriteriaErrorText);
        }
    }
}
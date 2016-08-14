namespace SFA.Apprenticeships.Web.Manage.UnitTests.Validators
{
    using Common.UnitTests.Validators;
    using FluentAssertions;
    using Manage.Validators;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    [Parallelizable]
    public class CandidateSearchResultsViewModelClientValidatorTests
    {
        private CandidateSearchResultsViewModelClientValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new CandidateSearchResultsViewModelClientValidator();
        }

        [Test]
        public void DefaultShouldNotHaveAnyValidationErrors()
        {
            var viewModel = new CandidateSearchResultsViewModel();

            var result = _validator.Validate(viewModel);

            result.IsValid.Should().BeTrue();
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void FirstNameInvalidCharacters(string firstName, bool expectValid)
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel
                {
                    FirstName = firstName,
                }
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.FirstName, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.FirstName, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", true)]
        [TestCase("<script>", false)]
        public void LastNameInvalidCharacters(string lastName, bool expectValid)
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel
                {
                    LastName = lastName,
                }
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.LastName, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.LastName, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("12/2/2016", false)]
        [TestCase("12/02/16", false)]
        [TestCase("12/02/2016", true)]
        public void DateOfBirthInvalidCharacters(string dateOfBirth, bool expectValid)
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel
                {
                    DateOfBirth = dateOfBirth,
                }
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.DateOfBirth, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.DateOfBirth, viewModel);
            }
        }

        [TestCase(null, true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("<script>", false)]
        [TestCase("CV1", false)]
        [TestCase("CV12", false)]
        [TestCase("CV12W", false)]
        [TestCase("CV12WT", true)]
        [TestCase("CV1 2WT", true)]
        public void PostcodeInvalidCharacters(string postcode, bool expectValid)
        {
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel
                {
                    Postcode = postcode
                }
            };

            _validator.Validate(viewModel);

            if (expectValid)
            {
                _validator.ShouldNotHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.Postcode, viewModel);
            }
            else
            {
                _validator.ShouldHaveValidationErrorFor(vm => vm.SearchViewModel, vm => vm.SearchViewModel.Postcode, viewModel);
            }
        }
    }
}
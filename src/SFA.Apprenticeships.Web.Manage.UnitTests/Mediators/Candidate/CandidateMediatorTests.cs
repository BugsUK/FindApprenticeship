namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Candidate
{
    using Common.UnitTests.Mediators;
    using Manage.Mediators.Candidate;
    using Manage.Validators;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    public class CandidateMediatorTests
    {
        private ICandidateMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = new CandidateMediator(new CandidateSearchResultsViewModelServerValidator());
        }

        [Test]
        public void ValidationFailedNullViewModel()
        {
            //Arrange
            var viewModel = new CandidateSearchResultsViewModel();

            //Act
            var result = _mediator.Search(viewModel);

            //Assert
            result.AssertValidationResult(CandidateMediatorCodes.Search.FailedValidation);
        }

        [Test]
        public void ValidationFailedNoSearchCriteria()
        {
            //Arrange
            var viewModel = new CandidateSearchResultsViewModel
            {
                SearchViewModel = new CandidateSearchViewModel()
            };

            //Act
            var result = _mediator.Search(viewModel);

            //Assert
            result.AssertValidationResult(CandidateMediatorCodes.Search.FailedValidation);
        }
    }
}
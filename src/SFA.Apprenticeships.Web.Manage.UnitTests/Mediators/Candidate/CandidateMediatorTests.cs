namespace SFA.Apprenticeships.Web.Manage.UnitTests.Mediators.Candidate
{
    using Common.UnitTests.Mediators;
    using Manage.Mediators.Candidate;
    using Manage.Providers;
    using Manage.Validators;
    using Moq;
    using NUnit.Framework;
    using Raa.Common.Providers;
    using ViewModels;

    [TestFixture]
    public class CandidateMediatorTests
    {
        private Mock<ICandidateProvider> _candidateProvider;
        private ICandidateMediator _mediator;
        
        [SetUp]
        public void SetUp()
        {
            _candidateProvider = new Mock<ICandidateProvider>();

            _mediator = new CandidateMediator(_candidateProvider.Object, new Mock<IVacancyQAProvider>().Object, new CandidateSearchViewModelServerValidator());
        }

        [Test]
        public void ValidationFailedNoSearchCriteria()
        {
            //Arrange
            var viewModel = new CandidateSearchViewModel();

            //Act
            var result = _mediator.Search(viewModel);

            //Assert
            result.AssertValidationResult(CandidateMediatorCodes.Search.FailedValidation);
        }

        [Test]
        public void Ok()
        {
            //Arrange
            var viewModel = new CandidateSearchViewModel
            {
                FirstName = "First"
            };

            //Act
            var result = _mediator.Search(viewModel);

            //Assert
            result.AssertCodeAndMessage(CandidateMediatorCodes.Search.Ok);
            _candidateProvider.Verify(p => p.SearchCandidates(viewModel));
        }
    }
}
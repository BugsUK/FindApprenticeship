namespace SFA.Apprenticeships.Web.Manage.UnitTests.Providers.Candidate
{
    using System;
    using Application.Interfaces.Candidates;
    using Manage.Providers;
    using Moq;
    using NUnit.Framework;
    using ViewModels;

    [TestFixture]
    public class CandidateProviderTests
    {
        private ICandidateProvider _provider;
        private Mock<ICandidateSearchService> _candidateSearchService;

        [SetUp]
        public void SetUp()
        {
            _candidateSearchService = new Mock<ICandidateSearchService>();

            _provider = new CandidateProvider(_candidateSearchService.Object);
        }

        [Test]
        public void TestConversion()
        {
            //Arrange
            var viewModel = new CandidateSearchViewModel
            {
                FirstName = "First",
                LastName = "Last",
                DateOfBirth = "10/02/1990",
                Postcode = "CV1 2WT"
            };

            //Act
            _provider.SearchCandidates(viewModel);

            //Assert
            var expectedDate = new DateTime(1990, 2, 10);
            _candidateSearchService.Verify(s => s.SearchCandidates(It.Is<CandidateSearchRequest>(r => 
                r.FirstName == viewModel.FirstName 
                && r.LastName == viewModel.LastName 
                && r.DateOfBirth == expectedDate 
                && r.Postcode == viewModel.Postcode)));
        }
    }
}
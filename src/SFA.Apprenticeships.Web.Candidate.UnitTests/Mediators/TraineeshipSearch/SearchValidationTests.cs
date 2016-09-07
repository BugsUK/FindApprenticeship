
namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipSearch
{
    using Candidate.Mediators.Search;
    using Candidate.ViewModels.VacancySearch;
    using Common.UnitTests.Mediators;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class SearchValidationTests : TestsBase
    {

        [Test]
        public void SearchByReferenceNumberValidationError()
        {
            var searchViewModel = new TraineeshipSearchViewModel
            {
                ReferenceNumber = string.Empty,
                Location = string.Empty
            };

            var response = Mediator.SearchValidation(searchViewModel);

            response.AssertValidationResult(TraineeshipSearchMediatorCodes.SearchValidation.ValidationError, true);
        }

        [Test]
        public void SearchByReferenceNumberValidationPass()
        {
            var searchViewModel = new TraineeshipSearchViewModel
            {
                ReferenceNumber = "VAC1234",
                Location = null
            };

            var response = Mediator.SearchValidation(searchViewModel);
            response.AssertCode(TraineeshipSearchMediatorCodes.SearchValidation.Ok, true);
        }
    }
}
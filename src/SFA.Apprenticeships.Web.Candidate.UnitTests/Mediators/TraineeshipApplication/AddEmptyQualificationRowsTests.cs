namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.UnitTests.Mediators;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class AddEmptyQualificationRowsTests : TestsBase
    {
        [Test]
        public void Ok()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyQualificationRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyQualificationRows.Ok, true);
        }
    }
}
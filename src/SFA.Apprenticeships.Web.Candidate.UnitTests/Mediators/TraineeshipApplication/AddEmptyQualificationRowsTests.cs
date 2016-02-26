using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Common.ViewModels.VacancySearch;
    using NUnit.Framework;

    [TestFixture]
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
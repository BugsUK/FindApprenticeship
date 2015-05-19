namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AddEmptyTrainingHistoryRowsTests : TestsBase
    {
        private const string BlankSpace = "  ";
        private const string SomeProvider = "Some provider";
        private const int SomeMonth = 1;
        private const string SomeYear = "2012";

        private string _someCourseTitle;

        [Test]
        public void Ok()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingHistoryRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyTrainingHistoryRows.Ok, true);
            response.ViewModel.Candidate.HasTrainingHistory.Should().BeFalse();
        }

        [Test]
        public void WillSetDefaultRowCounts()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingHistoryRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyTrainingHistoryRows.Ok, true);

            response.ViewModel.DefaultQualificationRows.Should().Be(0);
            response.ViewModel.DefaultWorkExperienceRows.Should().Be(0);
            response.ViewModel.DefaultTrainingHistoryRows.Should().Be(3);
        }

        [Test]
        public void WillRemoveEmptyTrainingHistoryRows()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = CreateCandidateWithOneTrainingHistoryRowAndTwoEmptyTrainingHistoryRows(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingHistoryRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyTrainingHistoryRows.Ok, true);
            response.ViewModel.Candidate.TrainingHistory.Should().HaveCount(1);
            response.ViewModel.Candidate.HasTrainingHistory.Should().BeTrue();
        }

        private TraineeshipCandidateViewModel CreateCandidateWithOneTrainingHistoryRowAndTwoEmptyTrainingHistoryRows()
        {
            _someCourseTitle = "Course title";

            return new TraineeshipCandidateViewModel
            {
                TrainingHistory = new[]
                {
                    new TrainingHistoryViewModel(),
                    new TrainingHistoryViewModel
                    {
                        Provider = SomeProvider,
                        FromMonth = SomeMonth,
                        FromYear = SomeYear,
                        CourseTitle = _someCourseTitle,
                        ToMonth = SomeMonth,
                        ToYear = SomeYear
                    },
                    new TrainingHistoryViewModel
                    {
                        Provider = BlankSpace,
                        FromMonth = SomeMonth,
                        FromYear = BlankSpace,
                        CourseTitle = BlankSpace,
                        ToMonth = SomeMonth,
                        ToYear = BlankSpace
                    }
                }
            };
        }
    }
}
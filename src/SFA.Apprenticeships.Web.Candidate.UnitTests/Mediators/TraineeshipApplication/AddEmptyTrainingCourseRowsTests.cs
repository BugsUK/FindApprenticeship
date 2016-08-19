namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.TraineeshipApplication
{
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using Common.UnitTests.Mediators;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class AddEmptyTrainingCourseRowsTests : TestsBase
    {
        private const string BlankSpace = "  ";
        private const string SomeProvider = "Some provider";
        private const int SomeMonth = 1;
        private const string SomeYear = "2012";

        private string _someTitle;

        private TraineeshipCandidateViewModel CreateCandidateWithOneTrainingCourseRowAndTwoEmptyTrainingCourseRows()
        {
            _someTitle = "Course title";

            return new TraineeshipCandidateViewModel
            {
                TrainingCourses = new[]
                {
                    new TrainingCourseViewModel(),
                    new TrainingCourseViewModel
                    {
                        Provider = SomeProvider,
                        FromMonth = SomeMonth,
                        FromYear = SomeYear,
                        Title = _someTitle,
                        ToMonth = SomeMonth,
                        ToYear = SomeYear
                    },
                    new TrainingCourseViewModel
                    {
                        Provider = BlankSpace,
                        FromMonth = SomeMonth,
                        FromYear = BlankSpace,
                        Title = BlankSpace,
                        ToMonth = SomeMonth,
                        ToYear = BlankSpace
                    }
                }
            };
        }

        [Test]
        public void Ok()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingCourseRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, true);
            response.ViewModel.Candidate.HasTrainingCourses.Should().BeFalse();
        }

        [Test]
        public void WillRemoveEmptyTrainingCourseRows()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = CreateCandidateWithOneTrainingCourseRowAndTwoEmptyTrainingCourseRows(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingCourseRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, true);
            response.ViewModel.Candidate.TrainingCourses.Should().HaveCount(1);
            response.ViewModel.Candidate.HasTrainingCourses.Should().BeTrue();
        }

        [Test]
        public void WillSetDefaultRowCounts()
        {
            var viewModel = new TraineeshipApplicationViewModel
            {
                Candidate = new TraineeshipCandidateViewModel(),
                VacancyDetail = new TraineeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingCourseRows(viewModel);

            response.AssertCode(TraineeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, true);

            response.ViewModel.DefaultQualificationRows.Should().Be(0);
            response.ViewModel.DefaultWorkExperienceRows.Should().Be(0);
            response.ViewModel.DefaultTrainingCourseRows.Should().Be(3);
        }
    }
}
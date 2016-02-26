using SFA.Apprenticeships.Web.Common.UnitTests.Mediators;

namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using Candidate.Mediators.Application;
    using Common.ViewModels.Applications;
    using Common.ViewModels.Candidate;
    using Common.ViewModels.VacancySearch;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AddEmptyTrainingCourseRowsTests : TestsBase
    {
        private const string BlankSpace = "  ";
        private const string SomeProvider = "Some provider";
        private const int SomeMonth = 1;
        private const string SomeYear = "2012";

        private string _someTitle;

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingCourseRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, true);
            response.ViewModel.Candidate.HasTrainingCourses.Should().BeFalse();
        }

        [Test]
        public void WillSetDefaultRowCounts()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingCourseRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, true);

            response.ViewModel.DefaultQualificationRows.Should().Be(0);
            response.ViewModel.DefaultWorkExperienceRows.Should().Be(0);
            response.ViewModel.DefaultTrainingCourseRows.Should().Be(3);
        }

        [Test]
        public void WillRemoveEmptyTrainingCourseRows()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = CreateCandidateWithOneTrainingCourseRowAndTwoEmptyTrainingCourseRows(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyTrainingCourseRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyTrainingCourseRows.Ok, true);
            response.ViewModel.Candidate.TrainingCourses.Should().HaveCount(1);
            response.ViewModel.Candidate.HasTrainingCourses.Should().BeTrue();
        }

        private ApprenticeshipCandidateViewModel CreateCandidateWithOneTrainingCourseRowAndTwoEmptyTrainingCourseRows()
        {
            _someTitle = "Course title";

            return new ApprenticeshipCandidateViewModel
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
    }
}
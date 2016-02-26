namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System;
    using Candidate.Views.Account;
    using Common.ViewModels.Applications;
    using Common.Views.Shared.DisplayTemplates;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class DashboardTraineeshipTests : ViewUnitTest
    {

        [TestCase(true)]
        [TestCase(false)]
        public void ShouldShowTraineeshipPrompt(bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsPrompt = shouldShow
                }).Build();

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("traineeshipPrompt");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
            }
            else
            {
                elem.Should().BeNull();
            }
        }


        [TestCase(true)]
        [TestCase(false)]
        public void ShouldShowFindTraineeshipLink(bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(new TraineeshipFeatureViewModel
                {
                    ShowTraineeshipsLink = shouldShow
                }).Build();

            // Act.
            var view = new Index().RenderAsHtml(myApplications);

            // Assert.
            var elem = view.GetElementbyId("find-traineeship-link");

            if (shouldShow)
            {
                elem.Should().NotBeNull();
            }
            else
            {
                elem.Should().BeNull();
            }
        }

        [TestCase(0,false)]
        [TestCase(2, true)]
        public void ShowTraineeships(int traineeshipCount, bool shouldShow)
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetTraineeships(traineeshipCount)).Build();

            // Act.
            var view = new MyApplications_().RenderAsHtml(myApplications);

            // Assert.
            var traineeshipsCount = view.GetElementbyId("traineeship-applications-count");
            var traineeshipsTable = view.GetElementbyId("dashTraineeships");

            if (shouldShow)
            {
                traineeshipsCount.Should().NotBeNull();
                traineeshipsCount.InnerHtml.Should().Be(Convert.ToString(traineeshipCount));
                traineeshipsTable.Should().NotBeNull();
            }
            else
            {
                traineeshipsCount.Should().BeNull();
                traineeshipsTable.Should().BeNull();
            }
        }

        [Test]
        public void ShowViewTraineeshipLink()
        {
            // Arrange.
            var myApplications =
                new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetTraineeships(3)).Build();

            // Act.
            var view = new MyApplications_().RenderAsHtml(myApplications);

            // Assert.
            foreach (var application in myApplications.TraineeshipApplications)
            {
                var id = string.Format("traineeship-view-link-{0}", application.VacancyId);
                var url = string.Format("traineeship/view/{0}", application.VacancyId);

                var viewTraineeshipLink = view.GetElementbyId(id);

                viewTraineeshipLink.Should().NotBeNull();
                viewTraineeshipLink.OuterHtml.Should().Contain(url);
            }
        }
    }
}
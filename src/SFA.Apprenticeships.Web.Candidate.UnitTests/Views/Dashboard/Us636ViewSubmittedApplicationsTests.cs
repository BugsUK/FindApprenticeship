namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Views.Dashboard
{
    using System.Linq;
    using Domain.Entities.Applications;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class Us636ViewSubmittedApplicationsTests
    {
        [Test]
        public void SuccessfulApplications()
        {
            var viewModel = new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(1, ApplicationStatuses.Successful)).Build();

            var result = new MyApplicationsViewBuilder().With(viewModel).Render();

            var dashSuccessful = result.GetElementbyId("dashSuccessful");
            dashSuccessful.Should().NotBeNull();

            var submittedTable = dashSuccessful.NextSibling.NextSibling;
            var submittedRow = submittedTable.ChildNodes.FindFirst("tbody").ChildNodes.FindFirst("tr");
            var submittedCells = submittedRow.ChildNodes.Where(n => n.Name == "td").ToList();

            submittedCells.Count.Should().Be(3);

            var title = submittedCells[0].ChildNodes.FindFirst("a");
            var viewApplication = submittedCells[1].ChildNodes.FindFirst("a");
            var untrackIcon = submittedCells[2].ChildNodes.FindFirst("a");

            title.Should().NotBeNull();
            viewApplication.Should().NotBeNull();
            untrackIcon.Should().NotBeNull();

            var href = viewApplication.Attributes.First(a => a.Name == "href");
            href.Value.Should().Be("/apprenticeship/view/" + viewModel.SuccessfulApprenticeshipApplications.First().VacancyId);
            viewApplication.InnerText.Should().Be("View application");
        }

        [Test]
        public void SubmittedApplications()
        {
            var viewModel = new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(1, ApplicationStatuses.Submitted)).Build();

            var result = new MyApplicationsViewBuilder().With(viewModel).Render();

            var dashSubmitted = result.GetElementbyId("dashSubmitted");
            dashSubmitted.Should().NotBeNull();

            var submittedTable = dashSubmitted.NextSibling.NextSibling;
            var submittedRow = submittedTable.ChildNodes.FindFirst("tbody").ChildNodes.FindFirst("tr");
            var submittedCells = submittedRow.ChildNodes.Where(n => n.Name == "td").ToList();

            submittedCells.Count.Should().Be(3);

            var title = submittedCells[0].ChildNodes.FindFirst("a");
            var viewApplication = submittedCells[1].ChildNodes.FindFirst("a");
            var submittedDate = submittedCells[2].InnerText;

            title.Should().NotBeNull();
            viewApplication.Should().NotBeNull();
            submittedDate.Should().NotBeNull();

            var href = viewApplication.Attributes.First(a => a.Name == "href");
            href.Value.Should().Be("/apprenticeship/view/" + viewModel.SubmittedApprenticeshipApplications.First().VacancyId);
            viewApplication.InnerText.Should().Be("View application");
        }

        [Test]
        public void UnsuccessfulApplications()
        {
            var viewModel = new MyApplicationsViewModelBuilder().With(DashboardTestsHelper.GetApprenticeships(1, ApplicationStatuses.Unsuccessful)).Build();

            var result = new MyApplicationsViewBuilder().With(viewModel).Render();

            var dashUnsuccessful = result.GetElementbyId("dashUnsuccessful");
            dashUnsuccessful.Should().NotBeNull();

            var submittedTable = dashUnsuccessful.NextSibling.NextSibling.NextSibling.NextSibling;
            var submittedRow = submittedTable.ChildNodes.FindFirst("tbody").ChildNodes.FindFirst("tr");
            var submittedCells = submittedRow.ChildNodes.Where(n => n.Name == "td").ToList();

            submittedCells.Count.Should().Be(3);

            var title = submittedCells[0].ChildNodes.FindFirst("a");
            var viewApplication = submittedCells[1].ChildNodes.FindFirst("a");
            var submittedDate = submittedCells[2].InnerText;

            title.Should().NotBeNull();
            viewApplication.Should().NotBeNull();
            submittedDate.Should().NotBeNull();

            var href = viewApplication.Attributes.First(a => a.Name == "href");
            href.Value.Should().Be("/apprenticeship/view/" + viewModel.UnsuccessfulApplications.First().VacancyId);
            viewApplication.InnerText.Should().Be("View application");
        }
    }
}
namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Builders;
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using RazorGenerator.Testing;

    [TestFixture]
    public class VacancyPreviewTests : ViewUnitTest
    {
        [Test]
        public void ApprenticeshipHeader()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft, VacancyType.Apprenticeship);
            var details = new VacancyPreview();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var heading = view.GetElementbyId("heading");
            heading.Should().NotBeNull();
            heading.InnerText.Should().Contain("Vacancy preview");
        }

        [Test]
        public void TraineeshipHeader()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft, VacancyType.Traineeship);
            var details = new VacancyPreview();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var heading = view.GetElementbyId("heading");
            heading.Should().NotBeNull();
            heading.InnerText.Should().Contain("Opportunity preview");
        }

        [Test]
        public void ApprenticeshipLongDescriptionHeader()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft, VacancyType.Apprenticeship);
            var details = new VacancyPreview();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var heading = view.GetElementbyId("long-description-header");
            heading.Should().NotBeNull();
            heading.InnerText.Should().Contain("Vacancy description");
        }

        [Test]
        public void TraineeshipLongDescriptionHeader()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft, VacancyType.Traineeship);
            var details = new VacancyPreview();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var heading = view.GetElementbyId("long-description-header");
            heading.Should().NotBeNull();
            heading.InnerText.Should().Contain("Work placement");
        }

        [Test]
        public void ApprenticeshipProviderInfoSection()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft, VacancyType.Apprenticeship);
            var details = new VacancyPreview();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("provider-info").Should().NotBeNull();
            view.GetElementbyId("about-traineeships").Should().BeNull();
        }

        [Test]
        public void TraineeshipAboutTraineeshipsSection()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(ProviderVacancyStatuses.Draft, VacancyType.Traineeship);
            var details = new VacancyPreview();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("provider-info").Should().BeNull();
            view.GetElementbyId("about-traineeships").Should().NotBeNull();
        }
    }
}
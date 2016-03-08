namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Builders;
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Raa.Vacancies;
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
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Apprenticeship);
            var details = new VacancyPreviewHeader();

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
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Traineeship);
            var details = new VacancyPreviewHeader();

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
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Apprenticeship);
            var details = new VacancyDetails();

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
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Traineeship);
            var details = new VacancyDetails();

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
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Apprenticeship);
            var details = new TrainingProvider();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("provider-info").Should().NotBeNull();
        }

        [Test]
        public void TraineeshipProviderInfoSection()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Traineeship);
            var details = new TrainingProvider();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("provider-info").Should().BeNull();
        }

        [Test]
        public void ApprenticeshipAboutTraineeshipsSection()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Apprenticeship);
            var details = new AboutTraineeships();

            //Act
            var view = details.RenderAsHtml(viewModel.VacancyType);

            //Assert
            view.GetElementbyId("about-traineeships").Should().BeNull();
        }

        [Test]
        public void TraineeshipAboutTraineeshipsSection()
        {
            //Arrange
            var viewModel = new VacancyViewModelBuilder().BuildValid(VacancyStatus.Draft, VacancyType.Traineeship);
            var details = new AboutTraineeships();

            //Act
            var view = details.RenderAsHtml(viewModel.VacancyType);

            //Assert
            view.GetElementbyId("about-traineeships").Should().NotBeNull();
        }
    }
}
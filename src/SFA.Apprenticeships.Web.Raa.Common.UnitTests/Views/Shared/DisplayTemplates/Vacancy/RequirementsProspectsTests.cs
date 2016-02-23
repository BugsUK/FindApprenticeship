namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using ViewModels.Vacancy;

    [TestFixture]
    public class RequirementsProspectsTests : ViewUnitTest
    {
        [Test]
        public void ApprenticeshipShouldHaveDesiredQualifications()
        {
            //Arrange
            var viewModel = new Fixture().Build<VacancyRequirementsProspectsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .Create();
            var details = new RequirementsProspects();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var desiredQualifications = view.GetElementbyId("DesiredQualifications");
            desiredQualifications.Should().NotBeNull();
            desiredQualifications.Attributes["type"].Value.Should().Be("text");
        }

        [Test]
        public void TraineeshipShouldNotHaveDesiredQualifications()
        {
            //Arrange
            var viewModel = new Fixture().Build<VacancyRequirementsProspectsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .Create();
            var details = new RequirementsProspects();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            var desiredQualifications = view.GetElementbyId("DesiredQualifications");
            desiredQualifications.Should().NotBeNull();
            desiredQualifications.Attributes["type"].Value.Should().Be("hidden");
            var desiredQualificationsComment = view.GetElementbyId("DesiredQualificationsComment");
            desiredQualificationsComment.Should().NotBeNull();
            desiredQualificationsComment.Attributes["type"].Value.Should().Be("hidden");
        }
    }
}
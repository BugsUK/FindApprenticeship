namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Views.Shared.DisplayTemplates.Vacancy
{
    using Common.Views.Shared.DisplayTemplates.Vacancy;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using RazorGenerator.Testing;
    using ViewModels.Vacancy;

    [TestFixture]
    public class TrainingDetailsTests : ViewUnitTest
    {
        [Test]
        public void ShouldHaveTrainingTypeSelectorWhenApprenticeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<TrainingDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .Create();
            var details = new TrainingDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("training-type-frameworks").Should().NotBeNull();
            view.GetElementbyId("training-type-standards").Should().NotBeNull();
        }

        [Test]
        public void ShouldHaveFrameworksAndStandardsSelectorsWhenApprenticeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<TrainingDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Apprenticeship)
                .Create();
            var details = new TrainingDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("training-type-frameworks-panel").Should().NotBeNull();
            view.GetElementbyId("training-type-frameworks-panel").Attributes["class"].Value.Should().Contain("toggle-content");

            view.GetElementbyId("training-type-standards-panel").Should().NotBeNull();
            view.GetElementbyId("training-type-standards-panel").Attributes["class"].Value.Should().Contain("toggle-content");

            view.GetElementbyId("FrameworkCodeName").Should().NotBeNull();
            view.GetElementbyId("StandardId").Should().NotBeNull();
        }

        [Test]
        public void ShouldNotHaveTrainingTypeSelectorWhenTraineeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<TrainingDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .Create();
            var details = new TrainingDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("training-type-frameworks").Should().BeNull();
            view.GetElementbyId("training-type-standards").Should().BeNull();
        }

        [Test]
        public void ShouldNotHaveFrameworkAndStandardsSelectorsWhenTraineeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<TrainingDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .Create();
            var details = new TrainingDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("training-type-frameworks-panel").Should().BeNull();
            view.GetElementbyId("training-type-standards-panel").Should().BeNull();
            view.GetElementbyId("FrameworkCodeName").Should().NotBeNull(); //Hidden field
            view.GetElementbyId("StandardId").Should().NotBeNull(); //Hidden field
            view.GetElementbyId("ApprenticeshipLevel").Should().NotBeNull(); //Hidden field
        }

        [Test]
        public void ShouldHaveSectorSelectorWhenTraineeship()
        {
            //Arrange
            var viewModel = new Fixture().Build<TrainingDetailsViewModel>()
                .With(vm => vm.VacancyType, VacancyType.Traineeship)
                .Create();
            var details = new TrainingDetails();

            //Act
            var view = details.RenderAsHtml(viewModel);

            //Assert
            view.GetElementbyId("SectorCodeName").Should().NotBeNull();
        }
    }
}
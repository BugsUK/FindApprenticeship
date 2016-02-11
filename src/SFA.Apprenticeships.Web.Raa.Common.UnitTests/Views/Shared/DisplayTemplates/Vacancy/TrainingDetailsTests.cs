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

            var frameworkCodeName = view.GetElementbyId("FrameworkCodeName");
            frameworkCodeName.Should().NotBeNull();
            frameworkCodeName.Name.Should().Be("select");
            var standardId = view.GetElementbyId("StandardId");
            standardId.Should().NotBeNull();
            standardId.Name.Should().Be("select");
            view.GetElementbyId("apprenticeship-level-intermediate").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level-advanced").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level-higher").Should().NotBeNull();
            view.GetElementbyId("apprenticeship-level-degree").Should().NotBeNull();
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
            var frameworkCodeName = view.GetElementbyId("FrameworkCodeName");
            frameworkCodeName.Should().NotBeNull();
            frameworkCodeName.Attributes["type"].Value.Should().Be("hidden");
            var standardId = view.GetElementbyId("StandardId");
            standardId.Should().NotBeNull();
            standardId.Attributes["type"].Value.Should().Be("hidden");
            var apprenticeshipLevel = view.GetElementbyId("ApprenticeshipLevel");
            apprenticeshipLevel.Should().NotBeNull();
            apprenticeshipLevel.Attributes["type"].Value.Should().Be("hidden");
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
            var sectorCodeName = view.GetElementbyId("SectorCodeName");
            sectorCodeName.Should().NotBeNull();
            sectorCodeName.Name.Should().Be("select");
        }
    }
}
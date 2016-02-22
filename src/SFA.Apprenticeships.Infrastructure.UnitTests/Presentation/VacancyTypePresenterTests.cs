namespace SFA.Apprenticeships.Infrastructure.UnitTests.Presentation
{
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using Infrastructure.Presentation;
    using NUnit.Framework;

    [TestFixture]
    public class VacancyTypePresenterTests
    {
        [Test]
        public void ApprenticeshipTitle()
        {
            //Arrange
            const string originalTitle = "Modern Apprenticeship";

            //Act
            var title = VacancyType.Apprenticeship.GetTitle(originalTitle);

            //Assert
            title.Should().Be(originalTitle);
        }

        [Test]
        public void TraineeshipTitle()
        {
            //Arrange
            const string originalTitle = "IT";

            //Act
            var title = VacancyType.Traineeship.GetTitle(originalTitle);

            //Assert
            title.Should().Be(originalTitle);
        }

        [Test]
        public void TraineeshipTitleAlreadyPrefixed()
        {
            //Arrange
            const string originalTitle = "Traineeship in IT";

            //Act
            var title = VacancyType.Traineeship.GetTitle(originalTitle);

            //Assert
            title.Should().Be(originalTitle);
        }
    }
}
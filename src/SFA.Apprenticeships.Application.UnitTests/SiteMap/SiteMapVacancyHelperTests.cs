namespace SFA.Apprenticeships.Application.UnitTests.SiteMap
{
    using System;
    using Apprenticeships.Application.Vacancy.SiteMap;
    using Domain.Entities.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class SiteMapVacancyHelperTests
    {
        [TestCase(VacancyType.Apprenticeship, 1, "https://www.findapprenticeship.service.gov.uk/apprenticeship/1")]
        [TestCase(VacancyType.Apprenticeship, 42, "https://www.findapprenticeship.service.gov.uk/apprenticeship/42")]
        [TestCase(VacancyType.Traineeship, 1, "https://www.findapprenticeship.service.gov.uk/traineeship/1")]
        [TestCase(VacancyType.Traineeship, 42, "https://www.findapprenticeship.service.gov.uk/traineeship/42")]
        public void ShouldConvertToUrl(VacancyType vacancyType, int vacancyId, string expectedUrl)
        {
            // Arrange.
            var siteMapVacancy = new SiteMapVacancy
            {
                VacancyId = vacancyId,
                VacancyType = vacancyType
            };

            // Act.
            var actualUrl = siteMapVacancy.ToUrl();

            // Assert.
            actualUrl.Should().Be(expectedUrl);
        }

        [Test]
        public void ShouldThrowIfUnknownVacancyType()
        {
            // Arrange.
            var siteMapVacancy = new SiteMapVacancy
            {
                VacancyId = 1,
                VacancyType = VacancyType.Unknown
            };

            // Act.
            Action action = () => siteMapVacancy.ToUrl();

            // Assert.
            action.ShouldThrow<ArgumentException>();
        }
    }
}

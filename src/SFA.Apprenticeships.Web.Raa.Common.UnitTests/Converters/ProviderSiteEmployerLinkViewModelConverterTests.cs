namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Converters
{
    using Common.Converters;
    using Domain.Entities.Raa.Parties;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    [Parallelizable]
    public class ProviderSiteEmployerLinkViewModelConverterTests
    {
        [TestCase("Anon Corp")]
        public void ShouldAnonymiseEmployerWhenVacancyIsAnonymised(string anonymousName)
        {
            // Arrange.
            var vacancyOwnerRelationship = new Fixture()
                .Create<VacancyOwnerRelationship>();

            var employer = new Fixture()
                .Create<Employer>();

            var vacancy = new Fixture().Create<Vacancy>();
            vacancy.EmployerAnonymousName = anonymousName;

            // Act.
            var converted = vacancyOwnerRelationship.Convert(employer, vacancy);

            // Assert.
            converted.Should().NotBeNull();
            converted.Employer.FullName.Should().Be(anonymousName);
        }

        [TestCase(null, "Null Corp")]
        [TestCase("", "Empty Corp")]
        [TestCase(" ", "Whitespace Corp")]
        public void ShouldNotAnonymiseEmployerWhenVacancyIsNotAnonymised(string anonymousName, string employerName)
        {
            // Arrange.
            var vacancyOwnerRelationship = new Fixture()
                .Create<VacancyOwnerRelationship>();

            var employer = new Fixture()
                .Build<Employer>()
                .With(each => each.FullName, employerName)
                .Create();

            var vacancy = new Fixture().Create<Vacancy>();
            vacancy.EmployerAnonymousName = anonymousName;
            // Act.
            var converted = vacancyOwnerRelationship.Convert(employer, vacancy);

            // Assert.
            converted.Should().NotBeNull();
            converted.Employer.FullName.Should().Be(employerName);
        }
    }
}

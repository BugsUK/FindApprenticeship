namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Converters
{
    using Common.Converters;
    using Domain.Entities.Raa.Parties;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    [TestFixture]
    public class ProviderSiteEmployerLinkViewModelConverterTests
    {
        [TestCase("Anon Corp")]
        public void ShouldAnonymiseEmployerWhenVacancyIsAnonymised(string anonymousName)
        {
            // Arrange.
            var vacancyParty = new Fixture()
                .Create<VacancyParty>();

            var employer = new Fixture()
                .Create<Employer>();

            // Act.
            var converted = vacancyParty.Convert(employer, anonymousName);

            // Assert.
            converted.Should().NotBeNull();
            converted.Employer.Name.Should().Be(anonymousName);
        }

        [TestCase(null, "Null Corp")]
        [TestCase("", "Empty Corp")]
        [TestCase(" ", "Whitespace Corp")]
        public void ShouldNotAnonymiseEmployerWhenVacancyIsNotAnonymised(string anonymousName, string employerName)
        {
            // Arrange.
            var vacancyParty = new Fixture()
                .Create<VacancyParty>();

            var employer = new Fixture()
                .Build<Employer>()
                .With(each => each.Name, employerName)
                .Create();

            // Act.
            var converted = vacancyParty.Convert(employer, anonymousName);

            // Assert.
            converted.Should().NotBeNull();
            converted.Employer.Name.Should().Be(employerName);
        }
    }
}

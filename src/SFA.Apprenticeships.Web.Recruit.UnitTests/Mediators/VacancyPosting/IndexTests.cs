namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using FluentAssertions;
    using NUnit.Framework;
    using Recruit.Providers;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [Test]
        public void ShouldGetNewVacancy()
        {
            // Arrange.
            var mediator = GetMediator();

            // Act.
            var viewModel = mediator.GetNewVacancyModel("john.doe@example.com");

            // Assert.
            // TODO: AG: US811: more assertions.
            viewModel.Should().NotBeNull();
        }
    }
}

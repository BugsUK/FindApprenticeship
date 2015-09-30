namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using NUnit.Framework;
    using Recruit.Providers;

    [TestFixture]
    public class IndexTests : TestsBase
    {
        [Test]
        [Ignore]
        public void ShouldGetNewVacancy()
        {
            // Arrange.
            var mediator = GetMediator();

            // Act.
            mediator.Index("john.doe@example.com");

            // Assert.
            Assert.Fail();
        }
    }
}

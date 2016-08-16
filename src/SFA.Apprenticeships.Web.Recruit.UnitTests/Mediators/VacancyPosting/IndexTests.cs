using System;

namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [Parallelizable]
    public class IndexTests : TestsBase
    {
        [Test]
        [Ignore("Review")]
        public void ShouldGetNewVacancy()
        {
            // Arrange.
            var mediator = GetMediator();

            // Act.
            var viewModel = mediator.GetNewVacancyViewModel(1123123, Guid.NewGuid(), null);

            // Assert.
            // TODO: AG: US811: more assertions.
            viewModel.Should().NotBeNull();
        }
    }
}

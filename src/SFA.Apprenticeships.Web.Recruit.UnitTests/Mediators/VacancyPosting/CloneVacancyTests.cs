namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Mediators.VacancyPosting
{
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Raa.Common.ViewModels.Vacancy;
    using Recruit.Mediators.VacancyPosting;

    [TestFixture]
    public class CloneVacancyTests : TestsBase
    {
        [Test]
        public void ShouldReturnVacancyInIncorrectStateErrorCodeIfVacancyIsInReferredStatus()
        {
            int vacancyReferenceNumber = 1;

            VacancyPostingProvider.Setup(p => p.GetVacancy(vacancyReferenceNumber)).Returns(new VacancyViewModel { Status = VacancyStatus.Referred});
            var mediator = GetMediator();

            var result = mediator.CloneVacancy(vacancyReferenceNumber);

            result.Code.Should().Be(VacancyPostingMediatorCodes.CloneVacancy.VacancyInIncorrectState);
        }
    }
}